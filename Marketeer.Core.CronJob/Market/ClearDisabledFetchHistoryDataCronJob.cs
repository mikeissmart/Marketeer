using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Core.CronJob.Market
{
    public class ClearDisabledFetchHistoryDataCronJob : BaseCronJobService
    {
        public ClearDisabledFetchHistoryDataCronJob(ICronJobConfig<ClearDisabledFetchHistoryDataCronJob> cronJobConfig,
            IServiceProvider services) : base(cronJobConfig, services)
        {

        }

        protected override async Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var tempDisabledFetchHistoryDataRepository = scope.ServiceProvider.GetRequiredService<ITempDisabledFetchHistoryDataRepository>();

            var all = await tempDisabledFetchHistoryDataRepository.GetAll();
            tempDisabledFetchHistoryDataRepository.RemoveRange(all);
            await tempDisabledFetchHistoryDataRepository.SaveChangesAsync();

            return $"Removed {all.Count}";
        }
    }
}
