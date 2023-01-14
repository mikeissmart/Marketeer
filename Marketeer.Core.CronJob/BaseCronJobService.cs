using Cronos;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.Repositories.Logging;
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

        public BaseCronJobService(CronConfig cronConfig, IServiceProvider services,
            string expressionName)
        {
            _services = services;
            _cronExpression = CronExpression.Parse(typeof(CronConfig).GetProperty(expressionName)!.GetValue(cronConfig)!.ToString());
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
        }

        public BaseCronJobService(CronConfig cronConfig, IServiceProvider services,
            string expressionName, TimeZoneInfo timeZoneInfo) : this(cronConfig, services, expressionName) => _timeZoneInfo = timeZoneInfo;

        public virtual async Task StartAsync(CancellationToken cancellationToken) => await ScheduleJob(cancellationToken);

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose() => _timer?.Dispose();

        protected abstract Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken);

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
                        var log = new CronLog()
                        {
                            Name = GetType().Name,
                            StartDate = DateTime.UtcNow,
                        };
                        using (var scope = _services.CreateScope())
                        {
                            log.Message = await DoWorkAsync(scope, cancellationToken);
                            log.EndDate = DateTime.UtcNow;
                            log.IsCanceled = cancellationToken.IsCancellationRequested;

                            var cronLogRepository = scope.ServiceProvider.GetRequiredService<ICronLogRepository>();
                            await cronLogRepository.AddAsync(log);
                            await cronLogRepository.SaveChangesAsync();
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
