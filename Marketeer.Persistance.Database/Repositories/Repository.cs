using Marketeer.Core.Domain;
using Marketeer.Core.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Marketeer.Persistance.Database.Repositories
{
    public interface IRepository
    {
        Task<T?> GetFirstOrDefaultAsync<T>(IQueryable<T> query) where T : class;
        Task<T?> GetFirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class;
        Task<T?> GetSingleOrDefaultAsync<T>(IQueryable<T> query) where T : class;
        Task<T?> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class;
        Task<List<T>> GetListAsync<T>(IQueryable<T> query) where T : class;
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class;
        Task<Paginate<T>> GetPaginateAsync<T>(PaginateFilterDto paginateFilter, IQueryable<T> query) where T : class;
        Task<Paginate<T>> GetPaginateAsync<T>(PaginateFilterDto paginateFilter, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class;
        Task<EntityEntry<T>> AddAsync<T>(T entity) where T : class;
        Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
        void UpdateState<T>(T entity) where T : class;
        EntityEntry<T> Update<T>(T entity) where T : class;
        void UpdateRange<T>(IEnumerable<T> entities) where T : class;
        EntityEntry<T> Remove<T>(T entity) where T : class;
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
        void Detach<T>(T entity) where T : class;
        Task SaveChangesAsync();
        IQueryable<T> GenerateQuery<T>(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, bool tracking) where T : class;
    }

    public abstract class Repository : IRepository
    {
        private readonly DbContext _dbContext;

        protected Repository(DbContext dbContext) => _dbContext = dbContext;

        public async Task<T?> GetFirstOrDefaultAsync<T>(IQueryable<T> query) where T : class
            => await query.FirstOrDefaultAsync();

        public async Task<T?> GetFirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class
            => await GenerateQuery(predicate, include, orderBy, tracking).FirstOrDefaultAsync();

        public async Task<T?> GetSingleOrDefaultAsync<T>(IQueryable<T> query) where T : class
            => await query.SingleOrDefaultAsync();

        public async Task<T?> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class
            => await GenerateQuery(predicate, include, orderBy, tracking).SingleOrDefaultAsync();

        public async Task<List<T>> GetListAsync<T>(IQueryable<T> query) where T : class
            => await query.ToListAsync();

        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class
            => await GenerateQuery(predicate, include, orderBy, tracking).ToListAsync();

        public async Task<Paginate<T>> GetPaginateAsync<T>(PaginateFilterDto paginateFilter, IQueryable<T> query) where T : class
        {
            var paginate = new Paginate<T>
            {
                PageIndex = paginateFilter.PageIndex,
                PageItemCount = paginateFilter.PageItemCount,
                TotalItemCount = await query.CountAsync()
            };

            if (paginateFilter.IsPaginated)
            {
                paginate.TotalPages = paginate.TotalItemCount / paginateFilter.PageItemCount + (
                    paginate.TotalItemCount % paginateFilter.PageItemCount > 0
                        ? 1
                        : 0);

                paginate.Items = await query
                    .Skip(paginateFilter.PageIndex * paginateFilter.PageItemCount)
                    .Take(paginateFilter.PageItemCount)
                    .ToListAsync();
            }
            else
            {
                paginate.TotalPages = 1;
                paginate.Items = await query
                    .ToListAsync();
            }

            return paginate;
        }

        public async Task<Paginate<T>> GetPaginateAsync<T>(PaginateFilterDto paginateFilter, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracking = true) where T : class
            => await GetPaginateAsync(paginateFilter, GenerateQuery(predicate, include, orderBy, tracking));

        public async Task<EntityEntry<T>> AddAsync<T>(T entity) where T : class
        => await _dbContext.Set<T>().AddAsync(entity);

        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
            => await _dbContext.Set<T>().AddRangeAsync(entities);

        public void UpdateState<T>(T entity) where T : class
            => _dbContext.Entry(entity).State = EntityState.Modified;

        public EntityEntry<T> Update<T>(T entity) where T : class
            => _dbContext.Set<T>().Update(entity);

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
            => _dbContext.Set<T>().UpdateRange(entities);

        public EntityEntry<T> Remove<T>(T entity) where T : class
            => _dbContext.Set<T>().Remove(entity);

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
            => _dbContext.Set<T>().RemoveRange(entities);

        public void Detach<T>(T entity) where T : class
            => _dbContext.Entry(entity).State = EntityState.Detached;

        public async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public IQueryable<T> GenerateQuery<T>(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, bool tracking) where T : class
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);
            if (include != null)
                query = include(query);
            if (orderBy != null)
                query = orderBy(query);
            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
