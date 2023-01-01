using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.Core.Domain.Entities.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.DbContexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int,
        AppUserClaim, AppUserRole, AppUserLogin,
        AppRoleClaim, AppUserToken>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options) =>
            _httpContextAccessor = httpContextAccessor;

        #region Logging

        public DbSet<AppLog> AppLogs => Set<AppLog>();
        public DbSet<CronLog> CronLogs => Set<CronLog>();
        public DbSet<PythonLog> PythonLogs => Set<PythonLog>();

        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int? userId = null;

            if (int.TryParse(_httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == "UserId")?.Value, out var temp))
                userId = temp;

            DbContextCommon.ChangeAuditableEntities(ChangeTracker, userId);

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbContextCommon.ConvertToUtc(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
