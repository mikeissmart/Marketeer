using Cronos;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Entities.CronJob;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.Repositories.CronJob;
using Marketeer.Persistance.Database.Repositories.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marketeer.Core.Cron
{
    public abstract class BaseCronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer? _timer;
        private readonly IServiceProvider _services;
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZoneInfo;

        public BaseCronJobService(CronJobConfig cronConfig, IServiceProvider services,
            string expressionName)
        {
            _services = services;
            _cronExpression = CronExpression.Parse(typeof(CronJobConfig).GetProperty(expressionName)!.GetValue(cronConfig)!.ToString());
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
        }

        public BaseCronJobService(CronJobConfig cronConfig, IServiceProvider services,
            string expressionName, TimeZoneInfo timeZoneInfo) : this(cronConfig, services, expressionName) => _timeZoneInfo = timeZoneInfo;

        public virtual async Task StartAsync(CancellationToken cancellationToken) => await ScheduleJob(cancellationToken);

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public void DoWorkNow()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Interval = 0.1;
                _timer.Start();
            }
        }

        public DateTimeOffset? NextOccurrence() => _cronExpression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

        public string GetCronExpression() => _cronExpression.ToString();

        public virtual void Dispose() => _timer?.Dispose();

        protected abstract Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken);

        private async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = NextOccurrence();
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
                            StartDateTime = DateTime.Now,
                        };
                        using (var scope = _services.CreateScope())
                        {
                            var cronJobStatusRepository = scope.ServiceProvider.GetRequiredService<ICronJobStatusRepository>();
                            var cronJobStatus = await cronJobStatusRepository.GetByNameAsync(GetType().Name);
                            if (cronJobStatus == null)
                            {
                                cronJobStatus = new CronJobStatus
                                {
                                    Name = GetType().Name,
                                    IsRunning = true
                                };
                                await cronJobStatusRepository.AddAsync(cronJobStatus);
                            }
                            else
                            {
                                cronJobStatus.IsRunning = true;
                                cronJobStatusRepository.Update(cronJobStatus);
                            }    
                            await cronJobStatusRepository.SaveChangesAsync();

                            try
                            {
                                log.Message = await DoWorkAsync(scope, cancellationToken);
                            }
                            catch (Exception ex)
                            {
                                // Consume error
                                ;
                            }
                            finally
                            {
                                log.EndDateTime = DateTime.Now;
                                log.IsCanceled = cancellationToken.IsCancellationRequested;

                                cronJobStatus.IsRunning = false;
                                cronJobStatusRepository.Update(cronJobStatus);
                                await cronJobStatusRepository.SaveChangesAsync();

                                var cronLogRepository = scope.ServiceProvider.GetRequiredService<ICronLogRepository>();
                                await cronLogRepository.AddAsync(log);
                                await cronLogRepository.SaveChangesAsync();
                            }
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
