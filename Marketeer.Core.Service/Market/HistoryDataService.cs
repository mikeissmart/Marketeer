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
                var query = _historyDataRepository.GetQuery(tickerId, HistoryDataIntervalEnum.One_Day);

                var summary = new TickerHistorySummaryDto
                {
                    ValueSummaries = new List<ValueSummaryDto>()
                };
                if (query.Count() > 0)
                {
                    DateTime? minDate = query.Count() > 0
                        ? query.Min(x => x.DateTime)
                        : null;
                    DateTime? maxDate = query.Count() > 0
                        ? query.Max(x => x.DateTime)
                        : null;
                    summary = new TickerHistorySummaryDto
                    {
                        ValueSummaries = new List<ValueSummaryDto>
                        {
                            GenerateSummary("Today", DateTime.UtcNow.Date, query),
                            GenerateSummary("7 Days", DateTime.UtcNow.Date.AddDays(-7), query),
                            GenerateSummary("1 Month", DateTime.UtcNow.Date.AddMonths(-1), query),
                            GenerateSummary("3 Months", DateTime.UtcNow.Date.AddMonths(-3), query),
                            GenerateSummary("6 Months", DateTime.UtcNow.Date.AddMonths(-6), query),
                            GenerateSummary("1 Year", DateTime.UtcNow.Date.AddYears(-1), query),
                            GenerateSummary("3 Years", DateTime.UtcNow.Date.AddYears(-3), query),
                            GenerateSummary("5 Years", DateTime.UtcNow.Date.AddYears(-5), query),
                        }
                    };
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

        private ValueSummaryDto GenerateSummary(string title, DateTime date, IEnumerable<HistoryData> query)
        {
            return new ValueSummaryDto
            {
                Title = title,
                Date = date,
                Value = query.FirstOrDefault(x => x.DateTime.Date == date.Date)?.Close
            };
        }

        private async Task<bool> FetchNewHistoryDataAsync(Ticker ticker, HistoryDataIntervalEnum interval, bool checkYfinanceRetry)
        {
            var retryHistDays = _tickerConfig.HistoryDataRetryDays;
            var today = DateTime.UtcNow.Date;

            var noHist = ticker.DelistReasons.FirstOrDefault(x => x.Delist == DelistEnum.Yfinance_No_History);

            if (checkYfinanceRetry)
            {
                if (noHist != null && noHist.CreatedDate.AddDays(retryHistDays) < today)
                    // Dont check for new history until after retryHistDays days
                    return false;
            }

            var curMaxDate = await _historyDataRepository.GetMaxDateTimeByTickerIntervalAsync(ticker.Id, interval);
            if (curMaxDate != null)
                // Inc date so duplicate history is not fetched
                curMaxDate = interval.AddInterval(curMaxDate.Value);

            var addedHistData = false;
            var marketDates = await _marketScheduleRepository.GetScheduleDaysInRangeAsync(curMaxDate, today);
            if (curMaxDate != null)
                marketDates = marketDates.Where(x => x.Day != curMaxDate.Value.Date).ToList();
            if (curMaxDate == null || marketDates.Count() > 0)
            {
                var freshHistDatas = _mapper.Map<IEnumerable<HistoryData>>(
                    await _marketPythonService.GetHistoryDataAsync(_mapper.Map<TickerDto>(ticker), interval,
                    curMaxDate, today));

                var addHistDatas = curMaxDate != null
                    ? freshHistDatas.Where(x => x.DateTime > curMaxDate)
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
                    !checkYfinanceRetry ||
                    ticker.LastHistoryUpdate.Value.AddDays(retryHistDays) < today)
                {
                    if (noHist != null)
                    {
                        noHist.CreatedDate = DateTime.UtcNow;
                        _tickerDelistReasonRepository.Update(noHist);
                    }
                    else
                        await _tickerDelistReasonRepository.AddAsync(new TickerDelistReason { Delist = DelistEnum.Yfinance_No_History });
                }

                ticker.LastHistoryUpdate = DateTime.UtcNow;
                _tickerRepository.Update(ticker);

                await _historyDataRepository.SaveChangesAsync();
            }

            return addedHistData;
        }
    }
}
