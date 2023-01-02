using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.DbContexts;

namespace Marketeer.Persistance.Database.Repositories.Logging
{
    public interface ICronLogRepository : IRepository<CronLog>
    {
        Task<List<CronLog>> GetLogsBerforeStartDateAsync(DateTime date);
    }

    public class CronLogRepository : BaseRepository<CronLog>, ICronLogRepository
    {
        public CronLogRepository(LogDbContext dbContext) : base(dbContext)
        { }

        public async Task<List<CronLog>> GetLogsBerforeStartDateAsync(DateTime date) =>
            await GetListAsync(x => x.StartDate < date);
    }
}
