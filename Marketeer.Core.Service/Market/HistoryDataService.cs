using AutoMapper;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Infrastructure.Python.Market;
using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Service.Market
{
    public interface IHistoryDataService : ICoreService
    {
        Task<List<HistoryDataDto>> GetHistoryDataAsync(int tickerId, HistoryDataIntervalEnum interval);
    }

    public class HistoryDataService : BaseCoreService, IHistoryDataService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HistoryDataService> _logger;
        private readonly IHistoryDataRepository _historyDataRepository;
        private readonly ITickerSettingsRepository _tickerSettingsRepository;
        private readonly ITickerRepository _tickerDataRepository;
        private readonly IYFinanceMarketData _yFinanceMarketData;

        public HistoryDataService(IMapper mapper,
            ILogger<HistoryDataService> logger,
            IHistoryDataRepository historyDataRepository,
            ITickerSettingsRepository tickerSettingsRepository,
            ITickerRepository tickerDataRepository,
            IYFinanceMarketData yFinanceMarketData)
        {
            _mapper = mapper;
            _logger = logger;
            _historyDataRepository = historyDataRepository;
            _tickerSettingsRepository = tickerSettingsRepository;
            _tickerDataRepository = tickerDataRepository;
            _yFinanceMarketData = yFinanceMarketData;
        }

        public async Task<List<HistoryDataDto>> GetHistoryDataAsync(int tickerId, HistoryDataIntervalEnum interval)
        {
            try
            {
                await GetLatestHistoryAsync(await _tickerDataRepository.GetTickerByIdAsync(tickerId), interval);

                return _mapper.Map<List<HistoryDataDto>>(await _historyDataRepository.GetHistoryDataByTickerIntervalDateRangeAsync(tickerId, interval));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        private async Task GetLatestHistoryAsync(Ticker? ticker, HistoryDataIntervalEnum interval)
        {
            if (ticker == null)
                throw new Exception("No Ticker.");

            if (await _tickerSettingsRepository.IsHistoryDisabled(ticker.Id, interval))
                return;

            var curMaxDate = await _historyDataRepository.GetMaxDateTimeByTickerIntervalAsync(ticker.Id, interval);
            var now = interval.MinusInterval(DateTime.UtcNow);
            if (curMaxDate <= now ||
                curMaxDate == null)
            {
                var freshHistDatas = _mapper.Map<IEnumerable<HistoryData>>(
                    await _yFinanceMarketData.GetHistoryDataAsync(_mapper.Map<TickerDto>(ticker), interval, curMaxDate, now));

                var addHistDatas = curMaxDate != null
                    ? freshHistDatas.Where(x => x.DateTime > curMaxDate)
                    : freshHistDatas;
                if (addHistDatas.Count() > 0)
                    await _historyDataRepository.AddRangeAsync(addHistDatas);

                var updateHistDatas = await _historyDataRepository.GetHistoryDataByTickerIntervalDateRangeAsync(ticker.Id, interval,
                    curMaxDate, null, false);
                var disableFetch = updateHistDatas.Count > 0;
                foreach (var histData in updateHistDatas)
                {
                    var updateData = freshHistDatas.FirstOrDefault(x => x.DateTime == histData.DateTime);
                    if (updateData != null)
                    {
                        if (updateData.Open != histData.Open ||
                            updateData.Close != histData.Close ||
                            updateData.High != histData.High ||
                            updateData.Low != histData.Low)
                            disableFetch = false;

                        updateData.Id = histData.Id;
                        _historyDataRepository.Update(updateData);
                    }
                }

                if (disableFetch)
                {
                    ticker.TickerSetting.TempHistoryDisable.Add(new TickerSettingHistoryDisable
                    {
                        TickerSettingId = ticker.TickerSetting.Id,
                        Interval = interval
                    });
                    _tickerSettingsRepository.Update(ticker.TickerSetting);
                }

                await _historyDataRepository.SaveChangesAsync();
            }
        }
    }
}
