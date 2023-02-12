using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITickerSettingsRepository : IRepository<TickerSetting>
    {
        Task<IEnumerable<TickerSetting>> GetAll();
        Task<bool> IsHistoryDisabled(int tickerId, HistoryDataIntervalEnum interval);
    }
    public class TickerSettingsRepository : BaseRepository<TickerSetting>, ITickerSettingsRepository
    {
        public TickerSettingsRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<IEnumerable<TickerSetting>> GetAll() =>
            await GetAsync(
                include: x => x
                    .Include(x => x.TempHistoryDisable));


        public async Task<bool> IsHistoryDisabled(int tickerId, HistoryDataIntervalEnum interval) =>
            await GenerateQuery(
                predicate: x =>
                    x.Ticker.Id == tickerId &&
                    x.TempHistoryDisable.Any(x => x.Interval == interval))
            .CountAsync() > 0;
    }
}
