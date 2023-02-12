using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Domain.InfrastructureDto.Python.Market;
using Marketeer.Core.Domain.InfrastructureDtos.Python.Market;
using Marketeer.Persistance.Database.Repositories.Logging;

namespace Marketeer.Infrastructure.Python.Market
{
    public interface IYFinanceMarketData : IPythonService
    {
        Task DownloadYFinanceTickerInfoAsync(List<string> tickers);
        Task<List<HistoryDataDto>> GetHistoryDataAsync(TickerDto ticker,
            HistoryDataIntervalEnum interval, DateTime? startDate, DateTime? endData);
    }

    public class YFinanceMarketData : BasePythonService, IYFinanceMarketData
    {
        private readonly YFinancePythonConfig _yFinanceConfig;
        private readonly IMapper _mapper;

        public YFinanceMarketData(YFinancePythonConfig yFinanceConfig,
            IMapper mapper,
            IPythonLogRepository pythonLogRepository) : base(yFinanceConfig, pythonLogRepository)
        {
            _yFinanceConfig = yFinanceConfig;
            _mapper = mapper;
        }

        public async Task DownloadYFinanceTickerInfoAsync(List<string> tickers)
        {
            var args = new PythonDownloadJsonInfoArgs
            {
                Tickers = tickers
            };
            await RunPythonScriptArgsAsync(
                _yFinanceConfig.DownloadTickerInfoJsonFile, args);
        }

        public async Task<List<HistoryDataDto>> GetHistoryDataAsync(TickerDto ticker,
            HistoryDataIntervalEnum interval, DateTime? startDate, DateTime? endData)
        {
            var args = new PythonHistoryDataArgs
            {
                Ticker = ticker.Symbol,
                Interval = interval.ToIntervalString(),
                StartDate = startDate?.ToString("yyyy-MM-dd"),
                EndDate = endData?.ToString("yyyy-MM-dd")
            };
            var histDatas = await RunPythonScriptAsync<List<HistoryDataDto>, PythonHistoryDataArgs>(
                _yFinanceConfig.HistoryDataFile, args);
            foreach (var hist in histDatas)
            {
                hist.TickerId = ticker.Id;
                hist.Interval = interval;
            }

            return histDatas;
        }
    }
}
