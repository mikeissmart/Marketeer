using Cronos;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Core.Service.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marketeer.Core.CronJob
{
    public abstract class BaseCronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer? _timer;
        private readonly IServiceProvider _services;
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZoneInfo;

        public BaseCronJobService(ICronJobConfig cronJobConfig, IServiceProvider services)
        {
            _services = services;
            _cronExpression = CronExpression.Parse(cronJobConfig.CronExpression);
            _timeZoneInfo = cronJobConfig.TimeZoneInfo;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken) => await ScheduleJob(cancellationToken);

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose() => _timer?.Dispose();

        protected abstract Task<string?> DoWork(IServiceScope scope, CancellationToken cancellationToken);

        private async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _cronExpression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next != null)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                    await ScheduleJob(cancellationToken);

                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();   // reset and dispose timer
                    _timer = null;
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        var log = new CronLogDto()
                        {
                            Name = GetType().Name,
                            StartDate = DateTime.UtcNow,
                        };
                        using (var scope = _services.CreateScope())
                        {
                            log.Message = await DoWork(scope, cancellationToken);
                            log.EndDate = DateTime.UtcNow;
                            log.IsCanceled = cancellationToken.IsCancellationRequested;

                            var cronLogService = scope.ServiceProvider.GetRequiredService<ICronLogService>();
                            await cronLogService.AddCronLogAsync(log);
                        }
                    }
                    if (!cancellationToken.IsCancellationRequested)
                        await ScheduleJob(cancellationToken);   // reschedule next

                };
                _timer.Start();
            }
            else
                await Task.CompletedTask;
        }
    }
}
