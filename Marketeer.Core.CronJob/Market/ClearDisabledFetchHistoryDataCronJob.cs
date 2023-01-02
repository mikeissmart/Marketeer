using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Core.CronJob.Market
{
    public class ClearDisabledFetchHistoryDataCronJob : BaseCronJobService
    {
        public ClearDisabledFetchHistoryDataCronJob(CronJobConfig<ClearDisabledFetchHistoryDataCronJob> cronJobConfig,
            IServiceProvider services) : base(cronJobConfig, services)
        {

        }

        protected override async Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var tempDisabledFetchHistoryDataRepository = scope.ServiceProvider.GetRequiredService<TempDisabledFetchHistoryDataRepository>();

            var all = await tempDisabledFetchHistoryDataRepository.GetAll();
            tempDisabledFetchHistoryDataRepository.RemoveRange(all);
            await tempDisabledFetchHistoryDataRepository.SaveChangesAsync();

            return $"Removed {all.Count}";
        }
    }
}
