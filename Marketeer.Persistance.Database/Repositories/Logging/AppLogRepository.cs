using Marketeer.Core.Domain;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Logging
{
    public interface IAppLogRepository : IRepository<AppLog>
    {
        Task<List<int>> AllEventIdsAsync();
        Task<Paginate<AppLog>> GetAllPaginatedLogsAsync(PaginateFilterDto<AppLogFilterDto> filter);
        Task<List<AppLog>> GetLogsBerforeDate(DateTime date);
    }

    public class AppLogRepository : BaseRepository<AppLog>, IAppLogRepository
    {
        public AppLogRepository(LogDbContext dbContext) : base(dbContext)
        { }

        public async Task<List<int>> AllEventIdsAsync() =>
            await GenerateQuery()
                .Select(x => x.EventId)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

        public async Task<Paginate<AppLog>> GetAllPaginatedLogsAsync(PaginateFilterDto<AppLogFilterDto> filter) => await GetPaginateAsync(
                filter,
                predicate: x =>
                    (filter.Filter.LogLevel == null || x.LogLevel == filter.Filter.LogLevel) &&
                    (filter.Filter.EventId == null || x.EventId == filter.Filter.EventId) &&
                    (string.IsNullOrEmpty(filter.Filter.EventName) || x.EventName!.Contains(filter.Filter.EventName)) &&
                    (filter.Filter.MinDate == null || x.CreatedDate < filter.Filter.MinDate.Value) &&
                    (filter.Filter.MaxDate == null || x.CreatedDate >= filter.Filter.MaxDate.Value),
                orderBy: CalculateOrderBy(filter));

        public async Task<List<AppLog>> GetLogsBerforeDate(DateTime date) =>
            await GetAsync(x => x.CreatedDate < date);

        private Func<IQueryable<AppLog>, IOrderedQueryable<AppLog>>? CalculateOrderBy(PaginateFilterDto filter)
        {
            Func<IQueryable<AppLog>, IOrderedQueryable<AppLog>>? orderBy = null;
            switch (filter.OrderBy)
            {
                case nameof(AppLog.LogLevel):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.LogLevel)
                        : x => x.OrderByDescending(x => x.LogLevel);
                    break;
                case nameof(AppLog.EventName):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.EventName)
                        : x => x.OrderByDescending(x => x.EventName);
                    break;
                case nameof(AppLog.Source):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Source)
                        : x => x.OrderByDescending(x => x.Source);
                    break;
                case nameof(AppLog.CreatedDate):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.CreatedDate)
                        : x => x.OrderByDescending(x => x.CreatedDate);
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
