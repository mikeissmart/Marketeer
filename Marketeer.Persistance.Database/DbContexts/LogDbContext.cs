using Marketeer.Core.Domain.Entities.Logging;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.DbContexts
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
        }

        public DbSet<AppLog> AppLogs => Set<AppLog>();
        public DbSet<CronLog> CronLogs => Set<CronLog>();
        public DbSet<PythonLog> PythonLogs => Set<PythonLog>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DbContextCommon.ChangeAuditableEntities(ChangeTracker, null);

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbContextCommon.ChageDecimalPrecision(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
