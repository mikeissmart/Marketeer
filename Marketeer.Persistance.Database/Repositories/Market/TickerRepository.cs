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
        Task<List<string>> SearchSymbolAsync(string symbol, int limit);
    }

    public class TickerRepository : BaseRepository<Ticker>, ITickerRepository
    {
        public TickerRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<List<Ticker>> GetAllTickersAsync() =>
            await GetListAsync(
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

        public async Task<List<string>> SearchSymbolAsync(string symbol, int limit) =>
            await GenerateQuery(
                predicate: x => x.Symbol.Contains(symbol),
                orderBy: x => x.OrderBy(x => x.Symbol))
            .Take(limit)
            .Select(x => x.Symbol)
            .ToListAsync();
    }
}
