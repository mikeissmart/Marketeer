using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.EntityConfig;
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

        #region Market
        public DbSet<HistoryData> HistoryDatas => Set<HistoryData>();
        public DbSet<Ticker> Tickers => Set<Ticker>();
        public DbSet<TickerInfo> TickerInfos => Set<TickerInfo>();
        public DbSet<TickerSettingHistoryDisable> TickerSettingHistoryDisables => Set<TickerSettingHistoryDisable>();
        public DbSet<TickerSetting> TickerSettings => Set<TickerSetting>();

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

            var efConfig = typeof(IEntityTypeConfiguration<>);
            var iConfig = typeof(IEntityConfig);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iConfig.IsAssignableFrom(x) &&
                    x != iConfig);

            var applyConfig = typeof(ModelBuilder).GetMethod("ApplyConfiguration")!;
            foreach (var type in types)
            {
                var generics = type
                    .GetInterfaces()
                    .First(x => x.Name == efConfig.Name)
                    .GetGenericArguments()[0];
                var genericApplyConfig = applyConfig.MakeGenericMethod(generics);

                genericApplyConfig.Invoke(modelBuilder, new[] { Activator.CreateInstance(type) });
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
