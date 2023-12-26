using Marketeer.Common.Configs;
using Marketeer.Core.Service.Market;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Cron.Market
{
    public class UpdateDailyHistoryDataCronJob : BaseCronJobService
    {
        public UpdateDailyHistoryDataCronJob(CronJobConfig cronConfig, IServiceProvider services) :
            base(cronConfig, services, nameof(CronJobConfig.UpdateDailyHistoryData))
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
