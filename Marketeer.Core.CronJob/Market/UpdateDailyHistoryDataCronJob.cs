using Marketeer.Common.Configs;
using Marketeer.Core.Service.Market;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.CronJob.Market
{
    public class UpdateDailyHistoryDataCronJob : BaseCronJobService
    {
        public UpdateDailyHistoryDataCronJob(CronConfig cronConfig, IServiceProvider services) :
            base(cronConfig, services, nameof(CronConfig.UpdateDailyHistoryData))
        {

        }

        protected override async Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var historyService = scope.ServiceProvider.GetRequiredService<IHistoryDataService>();
            await historyService.UpdateDailyHistoryDataAsync();

            return null;
        }
    }
}
