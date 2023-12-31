using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface IHistoryDataRepository : IRepository<HistoryData>
    {
        IEnumerable<HistoryData> GetQuery(int tickerId, HistoryDataIntervalEnum interval);
        Task<(DateTime?, DateTime?)> GetMinMaxDateTimeByTickerIntervalAsync(int tickerId, HistoryDataIntervalEnum interval);
        Task<List<HistoryData>> GetHistoryDataByTickerIntervalDateRangeAsync(int tickerId, HistoryDataIntervalEnum interval,
            DateTime? minDate = null, DateTime? maxDate = null, bool tracking = false);
        Task<List<HistoryData>> GetHistoryDatasByDateRangeAsync(DateTime? minDate, DateTime? maxDate);
        Task<int> DeleteHistoryDataBelowDateAync(DateTime date);
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

        public async Task<(DateTime?, DateTime?)> GetMinMaxDateTimeByTickerIntervalAsync(int tickerId, HistoryDataIntervalEnum interval)
        {
            DateTime? max = await GenerateQuery(
                    predicate: x =>
                        x.TickerId == tickerId &&
                        x.Interval == interval,
                    orderBy: x => x.OrderByDescending(x => x.Date))
                .Select(x => x.Date)
                .FirstOrDefaultAsync();
            if (max == DateTime.MinValue)
                max = null;
            DateTime? min = await GenerateQuery(
                    predicate: x =>
                        x.TickerId == tickerId &&
                        x.Interval == interval,
                    orderBy: x => x.OrderBy(x => x.Date))
                .Select(x => x.Date)
                .FirstOrDefaultAsync();
            if (min == DateTime.MinValue)
                min = null;

            return (min, max);
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

        public async Task<List<HistoryData>> GetHistoryDatasByDateRangeAsync(DateTime? minDate, DateTime? maxDate) =>
            await GetAsync(x =>
                (minDate == null || x.Date > minDate.Value) &&
                (maxDate == null || x.Date < maxDate.Value));

        public async Task<int> DeleteHistoryDataBelowDateAync(DateTime date) =>
            await RawSqlAsync($"delete from HistoryDatas where Date < '{date.Date.ToString("yyyy-MM-dd")}'");
    }
}
