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
        Task<bool> UpdateTickerHistoryDataAsync(int tickerId);
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
                foreach (var ticker in await _tickerRepository.GetTickersWithoutDelistReasons(
                    new List<DelistEnum>
                    {
                        DelistEnum.Nasdaq_Removed,
                        DelistEnum.Yfinance_No_Ticker,
                        DelistEnum.Yfinance_No_Info
                    }))
                {
                    await FetchNewHistoryDataAsync(ticker, HistoryDataIntervalEnum.One_Day, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateTickerHistoryDataAsync(int tickerId)
        {
            try
            {
                var ticker = await _tickerRepository.GetTickerByIdAsync(tickerId);
                if (ticker == null)
                    throw new ArgumentNullException("No Ticker Found");

                return await FetchNewHistoryDataAsync(ticker, HistoryDataIntervalEnum.One_Day, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<TickerHistorySummaryDto> GetTickerHistorySummaryAsync(int tickerId)
        {
            try
            {
                var latestMarket = await _marketScheduleRepository.GetLastestMarketDayAsync();
                var query = _historyDataRepository.GetQuery(tickerId, HistoryDataIntervalEnum.One_Day);

                var summary = new TickerHistorySummaryDto
                {
                    ValueSummaries = new List<ValueSummaryDto>()
                };
                if (query.Count() > 0)
                {
                    var latestHistData = query.FirstOrDefault(x => x.Date.Date <= latestMarket.Date);
                    if (latestHistData != null)
                    {
                        summary = new TickerHistorySummaryDto
                        {
                            ValueSummaries = new List<ValueSummaryDto>
                            {
                                GenerateSummary("Today", DateTime.Now.Date, latestHistData, query),
                                GenerateSummary("7 Days", DateTime.Now.Date.AddDays(-7), latestHistData, query),
                                GenerateSummary("1 Month", DateTime.Now.Date.AddMonths(-1), latestHistData, query),
                                GenerateSummary("3 Months", DateTime.Now.Date.AddMonths(-3), latestHistData, query),
                                GenerateSummary("6 Months", DateTime.Now.Date.AddMonths(-6), latestHistData, query),
                                GenerateSummary("1 Year", DateTime.Now.Date.AddYears(-1), latestHistData, query),
                                GenerateSummary("3 Years", DateTime.Now.Date.AddYears(-3), latestHistData, query),
                                GenerateSummary("5 Years", DateTime.Now.Date.AddYears(-5), latestHistData, query),
                            }
                        };
                    }
                }

                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<HistoryDataDto>> GetHistoryDataAsync(int tickerId, HistoryDataIntervalEnum interval)
        {
            try
            {
                return _mapper.Map<List<HistoryDataDto>>(await _historyDataRepository.GetHistoryDataByTickerIntervalDateRangeAsync(tickerId, interval));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private ValueSummaryDto GenerateSummary(string title, DateTime date, HistoryData? latestHistData, IEnumerable<HistoryData> query)
        {
            var data = query.FirstOrDefault(x => x.Date.Date <= date.Date);
            return new ValueSummaryDto
            {
                Title = title,
                Date = data?.Date,
                Value = data?.Close,
                Difference = latestHistData?.Close - data?.Close
            };
        }

        private async Task<bool> FetchNewHistoryDataAsync(Ticker ticker, HistoryDataIntervalEnum interval, bool checkYfinanceRetry)
        {
            var retryHistDays = _tickerConfig.HistoryDataRetryDays;
            var now = DateTime.Now;

            var noHist = ticker.DelistReasons.FirstOrDefault(x => x.Delist == DelistEnum.Yfinance_No_History);

            if (checkYfinanceRetry)
            {
                if (noHist != null && noHist.CreatedDate.AddDays(retryHistDays) < now)
                    // Dont check for new history until after retryHistDays days
                    return false;
            }

            var curMaxDate = await _historyDataRepository.GetMaxDateTimeByTickerIntervalAsync(ticker.Id, interval);
            if (curMaxDate != null)
                // Inc date so duplicate history is not fetched
                curMaxDate = interval.AddInterval(curMaxDate.Value);

            var addedHistData = false;
            var marketDates = await _marketScheduleRepository.GetScheduleDaysInRangeAsync(curMaxDate, now);
            if (curMaxDate == null || marketDates.Count(x => now > x.MarketClose) > 0)
            {
                var freshHistDatas = _mapper.Map<IEnumerable<HistoryData>>(
                    await _marketPythonService.GetHistoryDataAsync(_mapper.Map<TickerDto>(ticker), interval,
                    curMaxDate, interval.AddInterval(now)));

                var addHistDatas = curMaxDate != null
                    ? freshHistDatas.Where(x => x.Date >= curMaxDate)
                    : freshHistDatas;

                if (addHistDatas.Count() > 0)
                {
                    await _historyDataRepository.AddRangeAsync(addHistDatas);
                    if (noHist != null)
                        // Yfinance now has history
                        ticker.DelistReasons.Remove(noHist);
                    addedHistData = true;
                }
                else if (ticker.LastHistoryUpdate == null ||
                    checkYfinanceRetry ||
                    ticker.LastHistoryUpdate.Value.AddDays(retryHistDays) < now)
                {
                    if (noHist != null)
                    {
                        noHist.CreatedDate = DateTime.Now;
                        _tickerDelistReasonRepository.Update(noHist);
                    }
                    else
                        await _tickerDelistReasonRepository.AddAsync(new TickerDelistReason { Delist = DelistEnum.Yfinance_No_History });
                }
            }

            ticker.LastHistoryUpdate = DateTime.Now;
            _tickerRepository.Update(ticker);

            await _historyDataRepository.SaveChangesAsync();

            return addedHistData;
        }
    }
}
