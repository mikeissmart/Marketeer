using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Infrastructure.Python.Market;
using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Marketeer.Core.Service.Market
{
    public interface IHistoryDataService : ICoreService
    {
        Task UpdateDailyHistoryDataAsync();
        Task<TickerHistorySummaryDto> GetTickerHistorySummaryAsync(int tickerId);
        Task<List<HistoryDataDto>> GetHistoryDataAsync(int tickerId, HistoryDataIntervalEnum interval);
    }

    public class HistoryDataService : BaseCoreService, IHistoryDataService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HistoryDataService> _logger;
        private readonly TickerConfig _tickerConfig;
        private readonly IHistoryDataRepository _historyDataRepository;
        private readonly IMarketScheduleRepository _marketScheduleRepository;
        private readonly ITickerDelistReasonRepository _tickerDelistReasonRepository;
        private readonly ITickerRepository _tickerRepository;
        private readonly IMarketPythonService _marketPythonService;

        public HistoryDataService(IMapper mapper,
            ILogger<HistoryDataService> logger,
            TickerConfig tickerConfig,
            IHistoryDataRepository historyDataRepository,
            IMarketScheduleRepository marketScheduleRepository,
            ITickerDelistReasonRepository tickerDelistReasonRepository,
            ITickerRepository tickerRepository,
            IMarketPythonService marketPythonService)
        {
            _mapper = mapper;
            _logger = logger;
            _tickerConfig = tickerConfig;
            _historyDataRepository = historyDataRepository;
            _marketScheduleRepository = marketScheduleRepository;
            _tickerDelistReasonRepository = tickerDelistReasonRepository;
            _tickerRepository = tickerRepository;
            _marketPythonService = marketPythonService;
        }

        public async Task UpdateDailyHistoryDataAsync()
        {
            try
            {
                var noHistDays = _tickerConfig.HistoryDataRetryDays;
                var interval = HistoryDataIntervalEnum.One_Day;
                var today = DateTime.UtcNow.Date;

                foreach (var ticker in await _tickerRepository.GetTickersWithoutDelistReasons(
                    DelistEnum.Nasdaq_Removed,
                    DelistEnum.Yfinance_No_Ticker,
                    DelistEnum.Yfinance_No_Info))
                {
                    var noHist = ticker.DelistReasons.FirstOrDefault(x => x.Delist == DelistEnum.Yfinance_No_History);
                    if (noHist != null && noHist.CreatedDate.AddDays(noHistDays) < today)
                        // Dont check for new history until x days later
                        continue;

                    var curMaxDate = await _historyDataRepository.GetMaxDateTimeByTickerIntervalAsync(ticker.Id, interval);
                    if (curMaxDate != null)
                        // Inc date so duplicate history is not fetched
                        curMaxDate = interval.AddInterval(curMaxDate.Value);

                    if (curMaxDate == null || await _marketScheduleRepository.ScheduleDaysInRangeCountAsync(curMaxDate, today) > 0)
                    {
                        var freshHistDatas = _mapper.Map<IEnumerable<HistoryData>>(
                            await _marketPythonService.GetHistoryDataAsync(_mapper.Map<TickerDto>(ticker), interval,
                            curMaxDate, today));

                        var addHistDatas = curMaxDate != null
                            ? freshHistDatas.Where(x => x.DateTime > curMaxDate)
                            : freshHistDatas;

                        if (addHistDatas.Count() > 0)
                            await _historyDataRepository.AddRangeAsync(addHistDatas);
                        else if (ticker.LastHistoryUpdate == null ||
                            ticker.LastHistoryUpdate.Value.AddDays(noHistDays) < today)
                        {
                            if (noHist != null)
                            {
                                noHist.CreatedDate = DateTime.UtcNow;
                                _tickerDelistReasonRepository.Update(noHist);
                            }
                            else
                            {
                                await _tickerDelistReasonRepository.AddAsync(new TickerDelistReason { Delist = DelistEnum.Yfinance_No_History });
                            }
                        }

                        ticker.LastHistoryUpdate = DateTime.UtcNow;
                        _tickerRepository.Update(ticker);
                    }

                    await _historyDataRepository.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<TickerHistorySummaryDto> GetTickerHistorySummaryAsync(int tickerId)
        {
            try
            {
                var query = _historyDataRepository.GetIEnumerable(tickerId, HistoryDataIntervalEnum.One_Day);

                await Task.Run(() => { });

                var summary = new TickerHistorySummaryDto
                {
                    EarliestDate = query.Min(x => x.DateTime),
                    LatestDate = query.Max(x => x.DateTime),
                    ValueSummaries = new List<ValueSummaryDto>
                    {
                        new ValueSummaryDto("7 Days", query.Where(x => x.DateTime > DateTime.UtcNow.Date.AddDays(-7)).Select(x => x.Close)),
                        new ValueSummaryDto("1 Month", query.Where(x => x.DateTime > DateTime.UtcNow.Date.AddMonths(-1)).Select(x => x.Close)),
                        new ValueSummaryDto("3 Months", query.Where(x => x.DateTime > DateTime.UtcNow.Date.AddMonths(-3)).Select(x => x.Close)),
                        new ValueSummaryDto("6 Months", query.Where(x => x.DateTime > DateTime.UtcNow.Date.AddMonths(-6)).Select(x => x.Close)),
                        new ValueSummaryDto("YTD", query.Where(x => x.DateTime > new DateTime(DateTime.UtcNow.Year - 1, 1, 1)).Select(x => x.Close)),
                        new ValueSummaryDto("1 Year", query.Where(x => x.DateTime > DateTime.UtcNow.Date.AddYears(-1)).Select(x => x.Close)),
                        new ValueSummaryDto("3 Years", query.Where(x => x.DateTime > DateTime.UtcNow.Date.AddYears(-3)).Select(x => x.Close)),
                        new ValueSummaryDto("5 Years", query.Where(x => x.DateTime > DateTime.UtcNow.Date.AddYears(-5)).Select(x => x.Close)),
                        new ValueSummaryDto("All", query.Select(x => x.Close)),
                    }
                };

                return summary;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<HistoryDataDto>> GetHistoryDataAsync(int tickerId, HistoryDataIntervalEnum interval)
        {
            try
            {
                return _mapper.Map<List<HistoryDataDto>>(await _historyDataRepository.GetHistoryDataByTickerIntervalDateRangeAsync(tickerId, interval));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
