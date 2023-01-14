using Marketeer.Common.Configs;
using Marketeer.Core.Service.Market;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Core.CronJob.Market
{
    public class ClearTickerSettingsTempHistoryDisableCronJob : BaseCronJobService
    {
        public ClearTickerSettingsTempHistoryDisableCronJob(CronConfig cronConfig, IServiceProvider services) :
            base(cronConfig, services, nameof(CronConfig.ClearTickerSettingsTempHistoryDisable))
        {

        }

        protected override async Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var tickerService = scope.ServiceProvider.GetRequiredService<ITickerService>();
            await tickerService.ClearTickerSettingsTempHistoryDisableAsync();

            return null;
        }
    }
}
