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
        Task<int> PruneOlderThanMinHistoryDataAsync();
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
                var msft = await _tickerRepository.GetTickerByIdAsync(26118);
                await FetchNewHistoryDataAsync(msft, HistoryDataIntervalEnum.One_Day, true);
                /*foreach (var ticker in await _tickerRepository.GetWatchedTickersWithoutDelistReasonsAsync(
                    new List<DelistEnum>
                    {
                        DelistEnum.Nasdaq_Removed,
                        DelistEnum.Yfinance_No_Ticker,
                        DelistEnum.Yfinance_No_Info
                    }))
                {
                    await FetchNewHistoryDataAsync(ticker, HistoryDataIntervalEnum.One_Day, true);
                }*/
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> PruneOlderThanMinHistoryDataAsync()
        {
            try
            {
                return await _historyDataRepository.DeleteHistoryDataBelowDateAync(_tickerConfig.MinHistoryDataYearlyDate);
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
                                GenerateSummary("10 Years", DateTime.Now.Date.AddYears(-10), latestHistData, query),
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
            var tickerDto = _mapper.Map<TickerDto>(ticker);
            var retryHistDays = _tickerConfig.HistoryDataRetryDays;
            var noHist = ticker.DelistReasons.FirstOrDefault(x => x.Delist == DelistEnum.Yfinance_No_History);

            if (checkYfinanceRetry)
            {
                // TODO change CreateDate to CreatedDateTime
                if (noHist != null && noHist.CreatedDateTime.AddDays(retryHistDays) < DateTime.Now)
                    // Dont check for new history until after retryHistDays days
                    return false;
            }

            var success = false;
            var (curMinDate, curMaxDate) = await _historyDataRepository.GetMinMaxDateTimeByTickerIntervalAsync(ticker.Id, interval);
            if (curMinDate == null && curMaxDate == null)
            {
                // No hist data
                var fetchMinDate = _tickerConfig.MinHistoryDataYearlyDate.AddDays(-1);
                var fetchMaxDate = DateTime.Now.Date.AddDays(1);

                success = await AddFetchedHistoryDataAsync(tickerDto, interval, fetchMinDate, fetchMaxDate);
            }
            else
            {
                // Add older history datas
                var fetchMinDate = _tickerConfig.MinHistoryDataYearlyDate.AddDays(-1);
                var fetchMaxDate = curMinDate!.Value;

                success = await AddFetchedHistoryDataAsync(tickerDto, interval, fetchMinDate, fetchMaxDate);

                if (success)
                {
                    // Add new history datas
                    fetchMinDate = curMaxDate!.Value;
                    fetchMaxDate = DateTime.Now.Date;

                    success = await AddFetchedHistoryDataAsync(tickerDto, interval, fetchMinDate, fetchMaxDate);
                }
            }

            if (!success)
            {
                if (checkYfinanceRetry ||
                    ticker.LastHistoryUpdateDateTime == null ||
                    ticker.LastHistoryUpdateDateTime.Value.AddDays(retryHistDays) < DateTime.Now)
                {
                    if (noHist != null)
                    {
                        noHist.CreatedDateTime = DateTime.Now;
                        _tickerDelistReasonRepository.Update(noHist);
                    }
                    else
                        await _tickerDelistReasonRepository.AddAsync(new TickerDelistReason
                        {
                            TickerId = ticker.Id,
                            Delist = DelistEnum.Yfinance_No_History
                        });
                }
            }

            ticker.LastHistoryUpdateDateTime = DateTime.Now;
            _tickerRepository.Update(ticker);
            await _historyDataRepository.SaveChangesAsync();

            return success;
        }

        private async Task<bool> AddFetchedHistoryDataAsync(TickerDto ticker, HistoryDataIntervalEnum interval, DateTime minDate, DateTime maxDate)
        {
            try
            {
                var marketDates = await _marketScheduleRepository.GetScheduleDaysInRangeAsync(minDate, maxDate);
                if (marketDates.Count > 0)
                {
                    var freshHistDatas = await _marketPythonService.GetHistoryDataAsync(ticker, interval, marketDates.First().Date, marketDates.Last().Date);
                    await _historyDataRepository.AddRangeAsync(_mapper.Map<List<HistoryData>>(freshHistDatas));
                    await _historyDataRepository.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("symbol may be delisted"))
                    return false;
                else
                    throw;
            }
        }
    }
}
