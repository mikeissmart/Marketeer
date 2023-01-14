using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Domain.InfrastructureDto.Python.Market;
using Marketeer.Persistance.Database.Repositories.Logging;

namespace Marketeer.Infrastructure.Python.Market
{
    public interface IYFinanceMarketData : IPythonService
    {
        Task<List<TickerInfoDto>> GetTickerInfosAsync(IEnumerable<TickerInfoDto> tickerInfos);
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

        public async Task<List<TickerInfoDto>> GetTickerInfosAsync(IEnumerable<TickerInfoDto> tickerInfos)
        {
            var infos = new List<TickerInfoDto>();
            var countPerBatch = 200;
            var batchCount = tickerInfos.Count() / countPerBatch +
                (tickerInfos.Count() % countPerBatch != 0 ? 1 : 0);

            for (var i = 0; i < batchCount; i++)
            {
                var args = new PythonTickerInfosArgs
                {
                    TickerInfos = tickerInfos
                        .Skip(i * countPerBatch)
                        .Take(countPerBatch)
                        .ToList()
                };
                infos.AddRange(await RunPythonScriptAsync<List<TickerInfoDto>, PythonTickerInfosArgs>(
                    _yFinanceConfig.TickerInfosFile, args));
            }

            return infos;
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
