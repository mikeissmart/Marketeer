using Marketeer.Common.Configs;
using Marketeer.Core.Service.Market;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Core.CronJob.Market
{
    public class RefreshTickersAndInfoCronJob : BaseCronJobService
    {
        public RefreshTickersAndInfoCronJob(CronConfig cronConfig, IServiceProvider services) :
            base(cronConfig, services, nameof(CronConfig.RefreshTickers))
        {

        }

        protected override async Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var tickerService = scope.ServiceProvider.GetRequiredService<ITickerService>();
            var result = await tickerService.RefreshTickersAsync();
            var infoUpdated = await tickerService.UpdateTickerInfoAsync();

            return $"Added: {result.Item1}, Relisted: {result.Item2}, Delisted: {result.Item3}, InfoUpdated: {infoUpdated}";
        }
    }
}
