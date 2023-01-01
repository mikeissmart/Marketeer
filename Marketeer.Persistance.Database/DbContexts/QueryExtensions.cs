using Marketeer.Core.Domain;

namespace Marketeer.Persistance.Database.DbContexts
{
    public static class QueryExtensions
    {
        public static IQueryable<TEntity> IsSoftDeleted<TEntity>(this IQueryable<TEntity> query) where TEntity : EntityAuditRemove
        {
            query.Where(x => x.RemovedDate != null);

            return query;
        }

        public static IQueryable<TEntity> IsNotSoftDeleted<TEntity>(this IQueryable<TEntity> query) where TEntity : EntityAuditRemove
        {
            query.Where(x => x.RemovedDate == null);

            return query;
        }
    }
}
