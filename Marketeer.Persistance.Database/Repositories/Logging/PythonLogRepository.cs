using Marketeer.Core.Domain;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Logging
{
    public interface IPythonLogRepository : IRepository<PythonLog>
    {
        Task<List<string>> AllFilesAsync();
        Task<Paginate<PythonLog>> GetAllPaginatedLogsAsync(PaginateFilterDto<PythonLogFilterDto> filter);
        Task<List<PythonLog>> GetLogsBerforeDateAsync(DateTime date);
    }

    public class PythonLogRepository : BaseRepository<PythonLog>, IPythonLogRepository
    {
        public PythonLogRepository(LogDbContext dbContext) : base(dbContext)
        { }

        public async Task<List<string>> AllFilesAsync() =>
            await GenerateQuery()
                .Select(x => x.File)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

        public async Task<Paginate<PythonLog>> GetAllPaginatedLogsAsync(PaginateFilterDto<PythonLogFilterDto> filter) =>
            await GetPaginateAsync(
                filter,
                predicate: x =>
                    (filter.Filter.HasError == null || x.Error.Length > 0) &&
                    (string.IsNullOrEmpty(filter.Filter.File) || x.File.Contains(filter.Filter.File)) &&
                    (filter.Filter.MinDate == null || x.StartDate < filter.Filter.MinDate.Value) &&
                    (filter.Filter.MaxDate == null || x.StartDate >= filter.Filter.MaxDate.Value),
                orderBy: CalculateOrderBy(filter));

        public async Task<List<PythonLog>> GetLogsBerforeDateAsync(DateTime date) =>
            await GetAsync(x => x.StartDate < date);

        private Func<IQueryable<PythonLog>, IOrderedQueryable<PythonLog>>? CalculateOrderBy(PaginateFilterDto filter)
        {
            Func<IQueryable<PythonLog>, IOrderedQueryable<PythonLog>>? orderBy = null;
            switch (filter.OrderBy)
            {
                case nameof(PythonLog.File):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.File)
                        : x => x.OrderByDescending(x => x.File);
                    break;
                case nameof(PythonLog.StartDate):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.StartDate)
                        : x => x.OrderByDescending(x => x.StartDate);
                    break;
                case null:
                    orderBy = null;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return orderBy;
        }
    }
}
