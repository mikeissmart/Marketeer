using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.Persistance.Database.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.Auth
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        Task<AppUser?> GetUserByUserNameAsync(string userName);
    }

    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(AppDbContext dbContext) : base(dbContext)
        { }

        public async Task<AppUser?> GetUserByUserNameAsync(string userName) =>
            await GetSingleOrDefaultAsync(x => x.UserName == userName);
    }
}
