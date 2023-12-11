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
            GenerateQuery(x => x.TickerId == tickerId && x.Interval == interval);

        public async Task<DateTime?> GetMaxDateTimeByTickerIntervalAsync(int tickerId, HistoryDataIntervalEnum interval)
        {
            DateTime? value = await GenerateQuery(
                    predicate: x =>
                        x.TickerId == tickerId &&
                        x.Interval == interval,
                    orderBy: x => x.OrderByDescending(x => x.DateTime))
                .Select(x => x.DateTime)
                .FirstOrDefaultAsync();
            if (value == DateTime.MinValue)
                value = null;

            return value;
        }

        public async Task<List<HistoryData>> GetHistoryDataByTickerIntervalDateRangeAsync(int tickerId, HistoryDataIntervalEnum interval,
            DateTime? minDate = null, DateTime? maxDate = null, bool tracking = false) =>
            await GetAsync(
                predicate: x =>
                    x.TickerId == tickerId &&
                    x.Interval == interval &&
                    (minDate == null || x.DateTime >= minDate.Value) &&
                    (maxDate == null || x.DateTime <= maxDate.Value),
                orderBy: x => x.OrderBy(x => x.DateTime),
                tracking: tracking);
    }
}
