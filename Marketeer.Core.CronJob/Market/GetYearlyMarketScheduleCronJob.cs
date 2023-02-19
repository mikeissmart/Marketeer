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
    public class GetYearlyMarketScheduleCronJob : BaseCronJobService
    {
        public GetYearlyMarketScheduleCronJob(CronConfig cronConfig, IServiceProvider services) :
            base(cronConfig, services, nameof(CronConfig.GetYearlyMarketSchedule))
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
