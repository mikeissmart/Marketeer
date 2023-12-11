using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.Core.Domain.Entities.CronJob;
using Marketeer.Persistance.Database.DbContexts;
using Marketeer.Persistance.Database.Repositories.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.CronJob
{
    public interface ICronJobStatusRepository : IRepository<CronJobStatus>
    {
        Task<List<CronJobStatus>> GetAllAsync();
        Task<CronJobStatus?> GetByNameAsync(string name);
    }

    public class CronJobStatusRepository : BaseRepository<CronJobStatus>, ICronJobStatusRepository
    {
        public CronJobStatusRepository(AppDbContext dbContext) : base(dbContext)
        { }

        public async Task<List<CronJobStatus>> GetAllAsync() =>
            await GetAsync();

        public async Task<CronJobStatus?> GetByNameAsync(string name) =>
            await GetSingleOrDefaultAsync(x => x.Name == name);
    }
}
