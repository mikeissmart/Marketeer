using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
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
        private readonly INasdaqApiClient _nasdaqApiHttpClient;
        private readonly IMarketPythonService _marketPythonService;

        public TickerService(IMapper mapper,
            ILogger<TickerService> logger,
            TickerConfig tickerConfig,
            ITickerRepository tickerDataRepository,
            IJsonTickerInfoRepository jsonTickerInfoRepository,
            INasdaqApiClient nasdaqApiHttpClient,
            IMarketPythonService marketPythonService)
        {
            _mapper = mapper;
            _logger = logger;
            _tickerConfig = tickerConfig;
            _tickerRepository = tickerDataRepository;
            _jsonTickerInfoRepository = jsonTickerInfoRepository;
            _nasdaqApiHttpClient = nasdaqApiHttpClient;
            _marketPythonService = marketPythonService;
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
                        Symbol = x
                    })
                    .ToList();
                var relist = curTickers
                    .Where(x =>
                        freshTickers.Any(y => x.Symbol == y) &&
                        x.DelistReasons.Any(x => x.Delist == DelistEnum.Nasdaq_Removed))
                    .ToList();
                var delistTickers = curTickers
                    .Where(x =>
                        !freshTickers.Any(y => x.Symbol == y) &&
                        !x.DelistReasons.Any(x => x.Delist == DelistEnum.Nasdaq_Removed))
                    .ToList();

                foreach (var item in relist)
                {
                    item.DelistReasons = item.DelistReasons
                        .Where(x => x.Delist != DelistEnum.Nasdaq_Removed)
                        .ToList();
                }
                foreach (var item in delistTickers)
                {
                    item.DelistReasons.Add(new TickerDelistReason { Delist = DelistEnum.Nasdaq_Removed });
                }

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
                return await _tickerRepository.SearchNamesAsync(search, limit);
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
                return await _tickerRepository.SearchQuoteTypesAsync(search, limit);
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
                return await _tickerRepository.SearchSectorsAsync(search, limit);
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
                return await _tickerRepository.SearchIndustriesAsync(search, limit);
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
                var limitPercent = _tickerConfig.TickerInfoRefreshPercent;
                var limitCount = (int)Math.Min(600, await _tickerRepository.GetListedTickerCountAsync() * limitPercent);
                var infos = await _tickerRepository.GetOldestListedTickerInfosAsync(limitCount);
                await _marketPythonService.DownloadTickerJsonInfo(infos.Select(x => x.Symbol).ToList());
                var updatedCount = await UploadInfoJsonsAsync(infos);

                return updatedCount;
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

        private async Task<int> UploadInfoJsonsAsync(List<Ticker> tickers)
        {
            var now = DateTime.UtcNow.Date;
            var updatedCount = 0;
            foreach (var ticker in tickers)
            {
                var jInfo = await _jsonTickerInfoRepository.GetJsonTickerBySymbolAsync(ticker.Symbol);
                if (jInfo == null)
                {
                    if (!ticker.DelistReasons.Any(x => x.Delist == DelistEnum.Yfinance_No_Ticker))
                        ticker.DelistReasons.Add(new TickerDelistReason { Delist = DelistEnum.Yfinance_No_Ticker });
                    continue;
                }

                ticker.DelistReasons = ticker.DelistReasons
                    .Where(x => x.Delist != DelistEnum.Yfinance_No_Ticker)
                    .ToList();

                if (jInfo.InfoJson == "null")
                {
                    if (!ticker.DelistReasons.Any(x => x.Delist == DelistEnum.Yfinance_No_Info))
                        ticker.DelistReasons.Add(new TickerDelistReason { Delist = DelistEnum.Yfinance_No_Info });
                    continue;
                }
                else
                {
                    var jObj = JsonNode.Parse(jInfo.InfoJson)!;

                    ticker.Name = jObj["shortName"]?.GetValue<string>() ?? "";
                    if (ticker.Name.Length == 0)
                    {
                        if (!ticker.DelistReasons.Any(x => x.Delist == DelistEnum.Yfinance_No_Info))
                            ticker.DelistReasons.Add(new TickerDelistReason { Delist = DelistEnum.Yfinance_No_Info });
                    }
                    else
                    {
                        ticker.DelistReasons = ticker.DelistReasons
                            .Where(x => x.Delist != DelistEnum.Yfinance_No_Info)
                            .ToList();

                        ticker.QuoteType = jObj["quoteType"]!.GetValue<string>();
                        ticker.Exchange = jObj["exchange"]!.GetValue<string>();

                        ticker.MarketCap = jObj["marketCap"]?.GetValue<long>();
                        ticker.Sector = jObj["sector"]?.GetValue<string>();
                        ticker.Industry = jObj["industry"]?.GetValue<string>();
                        ticker.Volume = jObj["volume"]?.GetValue<long>();
                        ticker.PayoutRatio = jObj["payoutRatio"]?.GetValue<float>();
                        ticker.DividendRate = jObj["dividendRate"]?.GetValue<float>();
                    }
                }
                ticker.LastInfoUpdate = now;
                updatedCount++;

                _tickerRepository.Update(ticker);
            }
            await _tickerRepository.SaveChangesAsync();

            return updatedCount;
        }
    }
}
