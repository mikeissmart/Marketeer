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
        private readonly TickerConfig _tickerConfig;

        public GetYearlyMarketScheduleCronJob(CronJobConfig cronConfig, IServiceProvider services, TickerConfig tickerConfig) :
            base(cronConfig, services, nameof(CronJobConfig.GetYearlyMarketSchedule))
        {
            _tickerConfig = tickerConfig;
        }

        protected override async Task<string?> DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var scheduleService = scope.ServiceProvider.GetRequiredService<IMarketScheduleService>();

            await scheduleService.GetYearlyMarketSchedulesAsync(DateTime.Now.Year - _tickerConfig.HistoryDataKeepNowYearMinusYear, _tickerConfig.HistoryDataKeepNowYearMinusYear);

            return null;
        }
    }
}
