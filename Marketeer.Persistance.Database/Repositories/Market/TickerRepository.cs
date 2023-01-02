using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.DbContexts;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITickerRepository : IRepository<Ticker>
    {
        Task<Ticker?> GetTickerByIdAsync(int id);
        Task<Ticker?> GetTickerBySymbolAsync(string symbol);
    }

    public class TickerRepository : BaseRepository<Ticker>, ITickerRepository
    {
        public TickerRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<Ticker?> GetTickerByIdAsync(int id) =>
            await GetSingleOrDefaultAsync(x => x.Id == id);

        public async Task<Ticker?> GetTickerBySymbolAsync(string symbol) =>
            await GetSingleOrDefaultAsync(x => x.Symbol == symbol.ToUpper());
    }
}
