using Marketeer.Core.Domain;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITickerRepository : IRepository<Ticker>
    {
        Task<int> GetListedTickerCountAsync();
        Task<List<Ticker>> GetOldestListedTickerInfosAsync(int limitCount);
        Task<List<Ticker>> GetAllTickersAsync();
        Task<List<Ticker>> GetTickersWithoutDelistReasons(List<DelistEnum> delists);
        Task<Ticker?> GetTickerByIdAsync(int id, bool withNewsArticles = false);
        Task<List<Ticker>> GetTickersByIdsAsync(List<int> ids);
        Task<Ticker?> GetTickerBySymbolAsync(string symbol);
        Task<List<string>> SearchSymbolsAsync(string? search, int limit);
        Task<List<string>> SearchNamesAsync(string? search, int limit);
        Task<List<string>> SearchQuoteTypesAsync(string? search, int limit);
        Task<List<string>> SearchSectorsAsync(string? search, int limit);
        Task<List<string>> SearchIndustriesAsync(string? search, int limit);
        Task<Paginate<Ticker>> GetTickerByFilterAsync(PaginateFilterDto<TickerFilterDto> filter, int userId);
        Task<List<Ticker>> GetWatchedTickersWithoutDelistReasonsAsync(List<DelistEnum> delists);
    }

    public class TickerRepository : BaseRepository<Ticker>, ITickerRepository
    {
        public TickerRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<int> GetListedTickerCountAsync() =>
            await GenerateQuery(x => !x.IsDelisted).CountAsync();

        public async Task<List<Ticker>> GetOldestListedTickerInfosAsync(int limitCount) =>
            await GenerateQuery(
                predicate: x => !x.DelistReasons.Any(),
                orderBy: x => x.OrderBy(x => x.LastInfoUpdateDateTime),
                tracking: false)
                .Take(limitCount)
                .ToListAsync();

        public async Task<List<Ticker>> GetAllTickersAsync() =>
            await GetAsync(
                include:x => x
                    .Include(x => x.DelistReasons));

        public async Task<List<Ticker>> GetTickersWithoutDelistReasons(List<DelistEnum> delists) =>
            await GetAsync(x => x.DelistReasons.Any(x => delists.Contains(x.Delist)));

        public async Task<Ticker?> GetTickerByIdAsync(int id, bool withNewsArticles = false) =>
            await GetSingleOrDefaultAsync(x => x.Id == id,
                includes: new List<Func<IQueryable<Ticker>, IIncludableQueryable<Ticker, object>>?>
                {
                    x => x.Include(x => x.DelistReasons),
                    (withNewsArticles
                        ? x => x.Include(x => x.NewsArticles)
                        : null)
                });

        public async Task<List<Ticker>> GetTickersByIdsAsync(List<int> ids) =>
            await GetAsync(x => ids.Contains(x.Id));

        public async Task<Ticker?> GetTickerBySymbolAsync(string symbol) =>
            await GetSingleOrDefaultAsync(
                predicate: x => x.Symbol == symbol.ToUpper());

        public async Task<List<string>> SearchSymbolsAsync(string? search, int limit) =>
            await GenerateQuery(x => search == null || x.Symbol.Contains(search))
            .Select(x => x.Symbol)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<List<string>> SearchNamesAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.Name.Length > 0 &&
                (search == null || x.Name.Contains(search)))
            .Select(x => x.Name)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<List<string>> SearchQuoteTypesAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.QuoteType.Length > 0 &&
                (search == null || x.QuoteType.Contains(search)))
            .Select(x => x.QuoteType)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<List<string>> SearchSectorsAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.Sector != null &&
                x.Sector.Length > 0 &&
                (search == null || x.Sector.Contains(search)))
            .Select(x => x.Sector!)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<List<string>> SearchIndustriesAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.Industry != null &&
                x.Industry.Length > 0 &&
                (search == null || x.Industry.Contains(search)))
            .Select(x => x.Industry!)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<Paginate<Ticker>> GetTickerByFilterAsync(PaginateFilterDto<TickerFilterDto> filter, int userId) =>
            await GetPaginateAsync(
                filter,
                predicate: x =>
                    (filter.Filter.Name == null || x.Name.Contains(filter.Filter.Name)) &&
                    (filter.Filter.Symbol == null || x.Symbol.Contains(filter.Filter.Symbol)) &&
                    (filter.Filter.QuoteType == null || x.QuoteType.Contains(filter.Filter.QuoteType)) &&
                    (filter.Filter.Sector == null || x.Sector!.Contains(filter.Filter.Sector)) &&
                    (filter.Filter.Industry == null || x.Industry!.Contains(filter.Filter.Industry)) &&
                    (filter.Filter.IsListed == null || x.DelistReasons.Any() != filter.Filter.IsListed) &&
                    (filter.Filter.IsUserWatching == null || x.WatchTickers.Any(x => x.Id == userId) == filter.Filter.IsUserWatching),
                include: x => x
                    .Include(x => x.WatchTickers),
                orderBy: CalculateOrderBy(filter));

        public async Task<List<Ticker>> GetWatchedTickersWithoutDelistReasonsAsync(List<DelistEnum> delists) =>
            await GetAsync(x => x.WatchTickers.Any() && !x.DelistReasons.Any(x => delists.Contains(x.Delist)));

        private Func<IQueryable<Ticker>, IOrderedQueryable<Ticker>>? CalculateOrderBy(PaginateFilterDto filter)
        {
            Func<IQueryable<Ticker>, IOrderedQueryable<Ticker>>? orderBy = null;
            switch (filter.OrderBy)
            {
                case nameof(Ticker.Name):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Name)
                        : x => x.OrderByDescending(x => x.Name);
                    break;
                case nameof(Ticker.Symbol):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Symbol)
                        : x => x.OrderByDescending(x => x.Symbol);
                    break;
                case nameof(Ticker.QuoteType):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.QuoteType)
                        : x => x.OrderByDescending(x => x.QuoteType);
                    break;
                case nameof(Ticker.Sector):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Sector)
                        : x => x.OrderByDescending(x => x.Sector);
                    break;
                case nameof(Ticker.Industry):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Industry)
                        : x => x.OrderByDescending(x => x.Industry);
                    break;
                case nameof(Ticker.Id):
                case null:
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Id)
                        : x => x.OrderByDescending(x => x.Id);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return orderBy;
        }
    }
}
