using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Infrastructure.External.Market;
using Marketeer.Infrastructure.Python.Market;
using Marketeer.Persistance.Database.DbContexts;
using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace Marketeer.Core.Service.Market
{
    public interface ITickerService : ICoreService
    {
        /// <summary>
        /// AddedCount, RelistedCount, DelistedCount
        /// </summary>
        /// <returns></returns>
        Task<(int, int, int)> RefreshTickersAsync();
        Task ClearTickerSettingsTempHistoryDisableAsync();
        Task<TickerDto?> GetTickerBySymbolAsync(string symbol);
        Task<List<string>> SearchNamesAsync(string? search, int limit);
        Task<List<string>> SearchSymbolsAsync(string? search, int limit);
        Task<List<string>> SearchQuoteTypesAsync(string? search, int limit);
        Task<List<string>> SearchSectorsAsync(string? search, int limit);
        Task<List<string>> SearchIndustriesAsync(string? search, int limit);
        Task<int> UpdateTickerInfoAsync();
        Task<PaginateDto<TickerDto>> GetTickerDetailsAsync(PaginateFilterDto<TickerFilterDto> filter);
    }

    public class TickerService : BaseCoreService, ITickerService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TickerService> _logger;
        private readonly TickerConfig _tickerConfig;
        private readonly ITickerRepository _tickerRepository;
        private readonly IJsonTickerInfoRepository _jsonTickerInfoRepository;
        private readonly ITickerInfoRepository _tickerInfoRepository;
        private readonly ITickerSettingsRepository _tickerSettingsRepository;
        private readonly INasdaqApiClient _nasdaqApiHttpClient;
        private readonly IYFinanceMarketData _yFinanceMarketData;

        public TickerService(IMapper mapper,
            ILogger<TickerService> logger,
            TickerConfig tickerConfig,
            ITickerRepository tickerDataRepository,
            IJsonTickerInfoRepository jsonTickerInfoRepository,
            ITickerInfoRepository tickerInfoRepository,
            ITickerSettingsRepository tickerSettingsRepository,
            INasdaqApiClient nasdaqApiHttpClient,
            IYFinanceMarketData yFinanceMarketData)
        {
            _mapper = mapper;
            _logger = logger;
            _tickerConfig = tickerConfig;
            _tickerRepository = tickerDataRepository;
            _tickerInfoRepository = tickerInfoRepository;
            _jsonTickerInfoRepository = jsonTickerInfoRepository;
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
                        x.TickerSetting.IsDelisted)
                    .ToList();
                var delistTickers = curTickers
                    .Where(x => !freshTickers.Any(y => x.Symbol == y) &&
                        x.TickerSetting.IsDelisted)
                    .ToList();

                foreach (var item in relist)
                    item.TickerSetting.IsDelisted = false;
                foreach (var item in delistTickers)
                    item.TickerSetting.IsDelisted = true;

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

                return _mapper.Map<TickerDto?>(ticker);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<string>> SearchNamesAsync(string? search, int limit)
        {
            try
            {
                return await _tickerInfoRepository.SearchNamesAsync(search, limit);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<string>> SearchSymbolsAsync(string? search, int limit)
        {
            try
            {
                return await _tickerRepository.SearchSymbolsAsync(search, limit);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<string>> SearchQuoteTypesAsync(string? search, int limit)
        {
            try
            {
                return await _tickerInfoRepository.SearchQuoteTypesAsync(search, limit);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<string>> SearchSectorsAsync(string? search, int limit)
        {
            try
            {
                return await _tickerInfoRepository.SearchSectorsAsync(search, limit);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<string>> SearchIndustriesAsync(string? search, int limit)
        {
            try
            {
                return await _tickerInfoRepository.SearchIndustriesAsync(search, limit);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<int> UpdateTickerInfoAsync()
        {
            try
            {
                var limitCount = (int)Math.Min(600, await _tickerInfoRepository.ListedTickerCountAsync() * .05f);
                var infos = await _tickerInfoRepository.GetOldestListedTickerInfosAsync(limitCount);
                await _yFinanceMarketData.DownloadYFinanceTickerInfoAsync(infos.Select(x => x.Ticker.Symbol).ToList());
                await UploadInfoJsonsAsync(infos);

                return limitCount;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<PaginateDto<TickerDto>> GetTickerDetailsAsync(PaginateFilterDto<TickerFilterDto> filter)
        {
            try
            {
                return _mapper.Map<PaginateDto<TickerDto>>(await _tickerRepository.GetTickerDetailsAsync(filter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        private async Task UploadInfoJsonsAsync(List<TickerInfo> infos)
        {
            var now = DateTime.UtcNow.Date;
            foreach (var tInfo in infos)
            {
                var jInfo = await _jsonTickerInfoRepository.GetJsonTickerBySymbolAsync(tInfo.Ticker.Symbol);
                if (jInfo == null)
                    continue;

                if (jInfo.InfoJson == "null")
                {
                    tInfo.Ticker.TickerSetting.IsDelisted = true;
                    tInfo.Ticker.TickerSetting.UpdateDailyHistory = false;
                }
                else
                {
                    var jObj = JsonNode.Parse(jInfo.InfoJson)!;

                    tInfo.Name = jObj["shortName"]?.GetValue<string>() ?? "";
                    if (tInfo.Name.Length == 0)
                    {
                        tInfo.Ticker.TickerSetting.IsDelisted = true;
                        tInfo.Ticker.TickerSetting.UpdateDailyHistory = false;
                    }
                    else
                    {
                        tInfo.QuoteType = jObj["quoteType"]!.GetValue<string>();
                        tInfo.Exchange = jObj["exchange"]!.GetValue<string>();

                        tInfo.MarketCap = jObj["marketCap"]?.GetValue<long>();
                        tInfo.Sector = jObj["sector"]?.GetValue<string>();
                        tInfo.Industry = jObj["industry"]?.GetValue<string>();
                        tInfo.Volume = jObj["volume"]?.GetValue<long>();
                        tInfo.PayoutRatio = jObj["payoutRatio"]?.GetValue<float>();
                        tInfo.DividendRate = jObj["dividendRate"]?.GetValue<float>();
                    }
                }
                tInfo.UpdatedDate = now;

                _tickerInfoRepository.Update(tInfo);
            }
            await _tickerInfoRepository.SaveChangesAsync();
        }
    }
}
