using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.Auth
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        Task<AppUser?> GetUserByIdAsync(int id, bool withWatchTicker = false);
        Task<AppUser?> GetUserByUserNameAsync(string userName);
    }

    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(AppDbContext dbContext) : base(dbContext)
        { }

        public async Task<AppUser?> GetUserByIdAsync(int id, bool withWatchTicker = false) =>
            await GetSingleOrDefaultAsync(
                predicate: x => x.Id == id,
                includes: new List<Func<IQueryable<AppUser>, IIncludableQueryable<AppUser, object>>?>
                {
                    withWatchTicker
                        ? x => x.Include(x => x.WatchTickers)
                            .ThenInclude(x => x.Ticker)
                        : null,
                });

        public async Task<AppUser?> GetUserByUserNameAsync(string userName) =>
            await GetSingleOrDefaultAsync(x => x.UserName == userName);
    }
}
