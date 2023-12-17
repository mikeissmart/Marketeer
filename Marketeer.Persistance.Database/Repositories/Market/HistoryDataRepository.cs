using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface IHistoryDataRepository : IRepository<HistoryData>
    {
        IEnumerable<HistoryData> GetQuery(int tickerId, HistoryDataIntervalEnum interval);
        Task<DateTime?> GetMaxDateTimeByTickerIntervalAsync(int tickerId, HistoryDataIntervalEnum interval);
        Task<List<HistoryData>> GetHistoryDataByTickerIntervalDateRangeAsync(int tickerId, HistoryDataIntervalEnum interval,
            DateTime? minDate = null, DateTime? maxDate = null, bool tracking = false);
    }

    public class HistoryDataRepository : BaseRepository<HistoryData>, IHistoryDataRepository
    {
        public HistoryDataRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public IEnumerable<HistoryData> GetQuery(int tickerId, HistoryDataIntervalEnum interval) =>
            GenerateQuery(
                predicate: x => x.TickerId == tickerId && x.Interval == interval,
                orderBy: x => x.OrderByDescending(x => x.Date));

        public async Task<DateTime?> GetMaxDateTimeByTickerIntervalAsync(int tickerId, HistoryDataIntervalEnum interval)
        {
            return await GenerateQuery(
                    predicate: x =>
                        x.TickerId == tickerId &&
                        x.Interval == interval,
                    orderBy: x => x.OrderByDescending(x => x.Date))
                .Select(x => x.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<HistoryData>> GetHistoryDataByTickerIntervalDateRangeAsync(int tickerId, HistoryDataIntervalEnum interval,
            DateTime? minDate = null, DateTime? maxDate = null, bool tracking = false) =>
            await GetAsync(
                predicate: x =>
                    x.TickerId == tickerId &&
                    x.Interval == interval &&
                    (minDate == null || x.Date >= minDate.Value) &&
                    (maxDate == null || x.Date <= maxDate.Value),
                orderBy: x => x.OrderBy(x => x.Date),
                tracking: tracking);
    }
}
