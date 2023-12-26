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
    public class GetYearlyMarketScheduleCronJob : BaseCronJobService
    {
        public GetYearlyMarketScheduleCronJob(CronJobConfig cronConfig, IServiceProvider services) :
            base(cronConfig, services, nameof(CronJobConfig.GetYearlyMarketSchedule))
        {

        }

        protected override async Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var scheduleService = scope.ServiceProvider.GetRequiredService<IMarketScheduleService>();
            await scheduleService.GetYearlyMarketSchedulesAsync(2);

            return null;
        }
    }
}
