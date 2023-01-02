using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.DbContexts;

namespace Marketeer.Persistance.Database.Repositories.Logging
{
    public interface IPythonLogRepository : IRepository<PythonLog>
    {
    }

    public class PythonLogRepository : BaseRepository<PythonLog>, IPythonLogRepository
    {
        public PythonLogRepository(LogDbContext dbContext) : base(dbContext)
        { }
    }
}
