using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITickerInfoRepository : IRepository<TickerInfo>
    {
        Task<List<TickerInfo>> GetTickerInfosWithNullUpdateAsync();
        Task<List<TickerInfo>> GetTickerInfosByUpdateDaysAsync(int minusDays, int limitCount = 1000);
    }

    public class TickerInfoRepository : BaseRepository<TickerInfo>, ITickerInfoRepository
    {
        public TickerInfoRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<List<TickerInfo>> GetTickerInfosWithNullUpdateAsync() =>
            await GetListAsync(
                predicate: x => !x.IsDelisted &&
                    (x.UpdatedDate == null),
                tracking: false);

        public async Task<List<TickerInfo>> GetTickerInfosByUpdateDaysAsync(int minusDays, int limitCount = 1000) =>
            await GenerateQuery(
                predicate: x => !x.IsDelisted &&
                    (x.UpdatedDate != null && x.UpdatedDate.Value.Date <= DateTime.UtcNow.Date.AddDays(-minusDays)),
                tracking: false)
            .Take(limitCount)
            .ToListAsync();
    }
}
