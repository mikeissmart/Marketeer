using Marketeer.Persistance.Database.DbContexts;

namespace Marketeer.Persistance.Database.Repositories.Logging
{
    public interface ICronLogRepository : IRepository
    {
    }

    public class CronLogRepository : Repository, ICronLogRepository
    {
        public CronLogRepository(LogDbContext dbContext) : base(dbContext)
        { }
    }
}
