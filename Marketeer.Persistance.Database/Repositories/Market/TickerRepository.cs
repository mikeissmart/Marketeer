using Marketeer.Core.Domain;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITickerRepository : IRepository<Ticker>
    {
        Task<List<Ticker>> GetAllTickersAsync();
        Task<Ticker?> GetTickerByIdAsync(int id);
        Task<Ticker?> GetTickerBySymbolAsync(string symbol);
        Task<List<string>> SearchSymbolsAsync(string? search, int limit);
        Task<Paginate<Ticker>> GetTickerDetailsAsync(PaginateFilterDto<TickerFilterDto> filter);
    }

    public class TickerRepository : BaseRepository<Ticker>, ITickerRepository
    {
        public TickerRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<List<Ticker>> GetAllTickersAsync() =>
            await GetAsync(
                include: x => x
                    .Include(x => x.TickerInfo)
                    .Include(x => x.TickerSetting)
                        .ThenInclude(x => x.TempHistoryDisable));

        public async Task<Ticker?> GetTickerByIdAsync(int id) =>
            await GetSingleOrDefaultAsync(x => x.Id == id);

        public async Task<Ticker?> GetTickerBySymbolAsync(string symbol) =>
            await GetSingleOrDefaultAsync(
                predicate: x => x.Symbol == symbol.ToUpper(),
                include: x => x.Include(x => x.TickerInfo));

        public async Task<List<string>> SearchSymbolsAsync(string? search, int limit) =>
            await GenerateQuery(x => search == null || x.Symbol.Contains(search))
            .Select(x => x.Symbol)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<Paginate<Ticker>> GetTickerDetailsAsync(PaginateFilterDto<TickerFilterDto> filter) =>
            await GetPaginateAsync(
                filter,
                predicate: x =>
                    (filter.Filter.Name == null || x.TickerInfo.Name.Contains(filter.Filter.Name)) &&
                    (filter.Filter.Symbol == null || x.Symbol.Contains(filter.Filter.Symbol)) &&
                    (filter.Filter.QuoteType == null || x.TickerInfo.QuoteType.Contains(filter.Filter.QuoteType)) &&
                    (filter.Filter.Sector == null || x.TickerInfo.Sector!.Contains(filter.Filter.Sector)) &&
                    (filter.Filter.Industry == null || x.TickerInfo.Industry!.Contains(filter.Filter.Industry)) &&
                    (filter.Filter.IsDelisted == null || x.TickerSetting.IsDelisted == filter.Filter.IsDelisted),
                include: x => x
                    .Include(x => x.TickerInfo)
                    .Include(x => x.TickerSetting),
                orderBy: CalculateOrderBy(filter));

        private Func<IQueryable<Ticker>, IOrderedQueryable<Ticker>>? CalculateOrderBy(PaginateFilterDto filter)
        {
            Func<IQueryable<Ticker>, IOrderedQueryable<Ticker>>? orderBy = null;
            switch (filter.OrderBy)
            {
                case nameof(TickerInfo.Name):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.TickerInfo.Name)
                        : x => x.OrderByDescending(x => x.TickerInfo.Name);
                    break;
                case nameof(Ticker.Symbol):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Symbol)
                        : x => x.OrderByDescending(x => x.Symbol);
                    break;
                case nameof(TickerInfo.QuoteType):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.TickerInfo.QuoteType)
                        : x => x.OrderByDescending(x => x.TickerInfo.QuoteType);
                    break;
                case nameof(TickerInfo.Sector):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.TickerInfo.Sector)
                        : x => x.OrderByDescending(x => x.TickerInfo.Sector);
                    break;
                case nameof(TickerInfo.Industry):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.TickerInfo.Industry)
                        : x => x.OrderByDescending(x => x.TickerInfo.Industry);
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
