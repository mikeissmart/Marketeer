using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Persistance.Database.Repositories.Logging;

namespace Marketeer.Infrastructure.Python.Market
{
    public interface IYFinanceMarketData : IPythonService
    {
        Task<List<HistoryDataDto>> GetHistoryDataAsync(TickerDto ticker,
            HistoryDataIntervalEnum interval, DateTime? startDate, DateTime? endData);
    }

    public class YFinanceMarketData : BasePythonService, IYFinanceMarketData
    {
        private readonly YFinancePythonConfig _yFinanceConfig;

        public YFinanceMarketData(YFinancePythonConfig yFinanceConfig,
            IPythonLogRepository pythonLogRepository) : base(yFinanceConfig, pythonLogRepository) => _yFinanceConfig = yFinanceConfig;

        public async Task<List<HistoryDataDto>> GetHistoryDataAsync(TickerDto ticker,
            HistoryDataIntervalEnum interval, DateTime? startDate, DateTime? endData)
        {
            var scriptDto = new HistoryDataArgs
            {
                Ticker = ticker.Symbol,
                Interval = interval.ToIntervalString(),
                StartDate = startDate?.ToString("yyyy-MM-dd"),
                EndDate = endData?.ToString("yyyy-MM-dd")
            };
            var histDatas = await RunPythonScriptAsync<List<HistoryDataDto>, HistoryDataArgs>(
                _yFinanceConfig.HistoryDataFile, scriptDto);
            foreach (var hist in histDatas)
            {
                hist.TickerId = ticker.Id;
                hist.Interval = interval;
            }

            return histDatas;
        }

        #region Script Dtos

        private class HistoryDataArgs
        {
            public string Ticker { get; set; }
            public string Interval { get; set; }
            public string? StartDate { get; set; }
            public string? EndDate { get; set; }
        }

        #endregion
    }
}
