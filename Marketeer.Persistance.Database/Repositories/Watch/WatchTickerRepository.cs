using Marketeer.Core.Domain.Entities.Watch;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.Watch
{
    public interface IWatchTickerRepository : IRepository<WatchTicker>
    {
        Task<WatchTicker?> GetWatchTickerUpdateTickerDailiesByTickerIdAndUserAsync(int tickerId, int userId);
        Task<List<WatchTicker>> GetWatchTickerUpdateTickerDailiesByTickerIdsAsync(List<int> tickerIds);
        Task<int> GetUserWatchTickerCountAsync(int userId);
    }

    public class WatchTickerRepository : BaseRepository<WatchTicker>, IWatchTickerRepository
    {
        public WatchTickerRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<WatchTicker?> GetWatchTickerUpdateTickerDailiesByTickerIdAndUserAsync(int tickerId, int userId) =>
            await GetSingleOrDefaultAsync(x => x.Ticker.Id == tickerId && x.AppUserId == userId);

        public async Task<List<WatchTicker>> GetWatchTickerUpdateTickerDailiesByTickerIdsAsync(List<int> tickerIds) =>
            await GetAsync(x => tickerIds.Contains(x.Ticker.Id));

        public async Task<int> GetUserWatchTickerCountAsync(int userId) =>
            await GenerateQuery(x => x.AppUserId == userId).CountAsync();
    }
}
