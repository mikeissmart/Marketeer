using Marketeer.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Marketeer.Persistance.Database.DbContexts
{
    internal static class DbContextCommon
    {
        public static void ChageDecimalPrecision(ModelBuilder modelBuilder)
        {
            foreach (var p in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(decimal) || x.ClrType == typeof(decimal?))
                .Select(x => modelBuilder.Entity(x.DeclaringEntityType.Name).Property(x.Name)))
            {
                p.HasPrecision(28, 10);
            }
        }

        /// <summary>
        /// Dont use, When sending just dates to Angular it gets changed from UTC to local
        /// </summary>
        /// <param name="modelBuilder"></param>
        /*public static void ConvertToUtc(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                x => x, x => x.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(x, DateTimeKind.Utc)
                    : x.ToUniversalTime());

            foreach (var p in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(DateTime) || x.ClrType == typeof(DateTime?))
                .Select(x => modelBuilder.Entity(x.DeclaringEntityType.Name).Property(x.Name)))
            {
                p.HasConversion(dateTimeConverter);
            }
        }*/

        public static void ChangeAuditableEntities(ChangeTracker changeTracker, int? userId)
        {
            changeTracker.DetectChanges();
            var nowDt = DateTime.Now;

            if (userId == null && changeTracker.Entries().Any(x =>
                (x.State == EntityState.Added &&
                    (x.Entity is EntityUserAuditCreate ||
                     x.Entity is EntityUserAuditCreateUpdate ||
                     x.Entity is EntityUserAuditCreateRemove ||
                     x.Entity is EntityUserAuditCreateUpdateRemove)) ||
                (x.State == EntityState.Modified &&
                    (x.Entity is EntityUserAuditUpdate ||
                     x.Entity is EntityUserAuditCreateUpdate ||
                     x.Entity is EntityUserAuditUpdateRemove ||
                     x.Entity is EntityUserAuditCreateUpdateRemove)) ||
                (x.State == EntityState.Deleted &&
                    (x.Entity is EntityUserAuditRemove ||
                     x.Entity is EntityUserAuditCreateRemove ||
                     x.Entity is EntityUserAuditUpdateRemove ||
                     x.Entity is EntityUserAuditCreateUpdateRemove))))
                throw new ArgumentNullException("", "Unable to add/update/remove user auditable entities when no user is logged in.");

            foreach (var entry in changeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is EntityAuditCreate createAudit)
                        createAudit.CreatedDate = nowDt;
                    else if (entry.Entity is EntityAuditCreateUpdate createUpdateAudit)
                        createUpdateAudit.CreatedDate = nowDt;
                    else if (entry.Entity is EntityAuditCreateRemove createRemoveAudit)
                        createRemoveAudit.CreatedDate = nowDt;
                    else if (entry.Entity is EntityAuditCreateUpdateRemove createUpdateRemoveAudit)
                        createUpdateRemoveAudit.CreatedDate = nowDt;

                    else if (entry.Entity is EntityUserAuditCreate userCreateAudit)
                    {
                        userCreateAudit.CreatedDate = nowDt;
                        userCreateAudit.CreatedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditCreateUpdate userCreateUpdateAudit)
                    {
                        userCreateUpdateAudit.CreatedDate = nowDt;
                        userCreateUpdateAudit.CreatedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditCreateRemove userCreateRemoveAudit)
                    {
                        userCreateRemoveAudit.CreatedDate = nowDt;
                        userCreateRemoveAudit.CreatedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditCreateUpdateRemove userCreateUpdateRemoveAudit)
                    {
                        userCreateUpdateRemoveAudit.CreatedDate = nowDt;
                        userCreateUpdateRemoveAudit.CreatedByAppUserId = userId!.Value;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is EntityAuditUpdate updateAudit)
                        updateAudit.UpdatedDate = nowDt;
                    else if (entry.Entity is EntityAuditCreateUpdate createUpdateAudit)
                        createUpdateAudit.UpdatedDate = nowDt;
                    else if (entry.Entity is EntityAuditUpdateRemove updateRemoveAudit)
                        updateRemoveAudit.UpdatedDate = nowDt;
                    else if (entry.Entity is EntityAuditCreateUpdateRemove createUpdateRemoveAudit)
                        createUpdateRemoveAudit.UpdatedDate = nowDt;

                    else if (entry.Entity is EntityUserAuditUpdate userUpdateAudit)
                    {
                        userUpdateAudit.UpdatedDate = nowDt;
                        userUpdateAudit.UpdatedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditCreateUpdate userCreateUpdateAudit)
                    {
                        userCreateUpdateAudit.UpdatedDate = nowDt;
                        userCreateUpdateAudit.UpdatedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditUpdateRemove userUpdateRemoveAudit)
                    {
                        userUpdateRemoveAudit.UpdatedDate = nowDt;
                        userUpdateRemoveAudit.UpdatedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditCreateUpdateRemove userCreateUpdateRemoveAudit)
                    {
                        userCreateUpdateRemoveAudit.UpdatedDate = nowDt;
                        userCreateUpdateRemoveAudit.UpdatedByAppUserId = userId!.Value;
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // Soft delete
                    if (entry.Entity is EntityAuditRemove removeAudit)
                        removeAudit.RemovedDate = nowDt;
                    else if (entry.Entity is EntityAuditCreateRemove createRemoveAudit)
                        createRemoveAudit.RemovedDate = nowDt;
                    else if (entry.Entity is EntityAuditUpdateRemove updateRemoveAudit)
                        updateRemoveAudit.RemovedDate = nowDt;
                    else if (entry.Entity is EntityAuditCreateUpdateRemove createUpdateRemoveAudit)
                        createUpdateRemoveAudit.RemovedDate = nowDt;

                    else if (entry.Entity is EntityUserAuditRemove userRemoveAudit)
                    {
                        userRemoveAudit.RemovedDate = nowDt;
                        userRemoveAudit.RemovedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditCreateRemove userCreateRemoveAudit)
                    {
                        userCreateRemoveAudit.RemovedDate = nowDt;
                        userCreateRemoveAudit.RemovedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditUpdateRemove userUpdateRemoveAudit)
                    {
                        userUpdateRemoveAudit.RemovedDate = nowDt;
                        userUpdateRemoveAudit.RemovedByAppUserId = userId!.Value;
                    }
                    else if (entry.Entity is EntityUserAuditCreateUpdateRemove userCreateUpdateRemoveAudit)
                    {
                        userCreateUpdateRemoveAudit.RemovedDate = nowDt;
                        userCreateUpdateRemoveAudit.RemovedByAppUserId = userId!.Value;
                    }

                    if (entry.Entity is EntityAuditRemove ||
                        entry.Entity is EntityAuditCreateRemove ||
                        entry.Entity is EntityAuditUpdateRemove ||
                        entry.Entity is EntityAuditCreateUpdateRemove ||
                        entry.Entity is EntityUserAuditRemove ||
                        entry.Entity is EntityUserAuditCreateRemove ||
                        entry.Entity is EntityUserAuditUpdateRemove ||
                        entry.Entity is EntityUserAuditCreateUpdateRemove)
                        entry.State = EntityState.Modified;
                }
            }
        }
    }
}
