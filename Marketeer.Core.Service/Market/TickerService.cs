using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Infrastructure.External.Market;
using Marketeer.Infrastructure.Python.Market;
using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Service.Market
{
    public interface ITickerService : ICoreService
    {
        /// <summary>
        /// AddedCount, RelistedCount, DelistedCount
        /// </summary>
        /// <returns></returns>
        Task<(int, int, int)> RefreshTickersAsync();
        Task<int> RefreshTickerInfosAsync();
        Task ClearTickerSettingsTempHistoryDisableAsync();
        Task<TickerDto?> GetTickerBySymbolAsync(string symbol);
        Task<List<string>> SearchSymbolAsync(string symbol, int limit);
    }

    public class TickerService : BaseCoreService, ITickerService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TickerService> _logger;
        private readonly TickerConfig _tickerConfig;
        private readonly ITickerRepository _tickerRepository;
        private readonly ITickerInfoRepository _tickerInfoRepository;
        private readonly ITickerSettingsRepository _tickerSettingsRepository;
        private readonly INasdaqApiHttpClient _nasdaqApiHttpClient;
        private readonly IYFinanceMarketData _yFinanceMarketData;

        public TickerService(IMapper mapper,
            ILogger<TickerService> logger,
            TickerConfig tickerConfig,
            ITickerRepository tickerDataRepository,
            ITickerInfoRepository tickerInfoRepository,
            ITickerSettingsRepository tickerSettingsRepository,
            INasdaqApiHttpClient nasdaqApiHttpClient,
            IYFinanceMarketData yFinanceMarketData)
        {
            _mapper = mapper;
            _logger = logger;
            _tickerConfig = tickerConfig;
            _tickerRepository = tickerDataRepository;
            _tickerInfoRepository = tickerInfoRepository;
            _tickerSettingsRepository = tickerSettingsRepository;
            _nasdaqApiHttpClient = nasdaqApiHttpClient;
            _yFinanceMarketData = yFinanceMarketData;
        }

        public async Task<(int, int, int)> RefreshTickersAsync()
        {
            try
            {
                var freshTickers = await _nasdaqApiHttpClient.AllSymbolsAsync();
                var curTickers = await _tickerRepository.GetAllTickersAsync();

                var addTickers = freshTickers
                    .Where(x => !curTickers.Any(y => x == y.Symbol))
                    .Select(x => new Ticker
                    {
                        Symbol = x,
                        TickerInfo = new TickerInfo(),
                        TickerSetting = new TickerSetting()
                    })
                    .ToList();
                var relist = curTickers
                    .Where(x => freshTickers.Any(y => x.Symbol == y) &&
                        x.TickerInfo.IsDelisted)
                    .ToList();
                var delistTickers = curTickers
                    .Where(x => !freshTickers.Any(y => x.Symbol == y) &&
                        x.TickerInfo.IsDelisted)
                    .ToList();

                foreach (var item in relist)
                    item.TickerInfo.IsDelisted = false;
                foreach (var item in delistTickers)
                    item.TickerInfo.IsDelisted = true;

                await _tickerRepository.AddRangeAsync(addTickers);
                _tickerRepository.UpdateRange(relist);
                _tickerRepository.UpdateRange(delistTickers);

                await _tickerRepository.SaveChangesAsync();

                return (addTickers.Count(), relist.Count(), delistTickers.Count());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<int> RefreshTickerInfosAsync()
        {
            try
            {
                // TODO yFinance info is broken
                var updateInfos = await _tickerInfoRepository.GetTickerInfosWithNullUpdateAsync();
                updateInfos.AddRange(await _tickerInfoRepository.GetTickerInfosByUpdateDaysAsync(_tickerConfig.InfoRefreshAfterDays, 1000));

                var infos = _mapper.Map<List<TickerInfo>>(
                    await _yFinanceMarketData.GetTickerInfosAsync(_mapper.Map<List<TickerInfoDto>>(updateInfos)));

                _tickerInfoRepository.UpdateRange(infos);
                await _tickerInfoRepository.SaveChangesAsync();

                return infos.Count;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task ClearTickerSettingsTempHistoryDisableAsync()
        {
            try
            {
                var settings = await _tickerSettingsRepository.GetAll();

                foreach (var item in settings)
                    item.TempHistoryDisable.Clear();

                await _tickerInfoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<TickerDto?> GetTickerBySymbolAsync(string symbol)
        {
            try
            {
                var ticker = await _tickerRepository.GetTickerBySymbolAsync(symbol);
                if (ticker != null)
                    await CheckTickerInfoRefresh(ticker.TickerInfo);

                return _mapper.Map<TickerDto?>(ticker);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<string>> SearchSymbolAsync(string symbol, int limit)
        {
            try
            {
                return await _tickerRepository.SearchSymbolAsync(symbol, limit);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        private async Task CheckTickerInfoRefresh(TickerInfo tickerInfo)
        {

        }
    }
}
