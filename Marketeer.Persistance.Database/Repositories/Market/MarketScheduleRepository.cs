using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface IMarketScheduleRepository : IRepository<MarketSchedule>
    {
        Task<List<MarketSchedule>> GetScheduleDaysInRangeAsync(DateTime? minDate, DateTime? maxDate);
        Task<MarketSchedule?> GetLastestMarketDayAsync();
    }

    public class MarketScheduleRepository : BaseRepository<MarketSchedule>, IMarketScheduleRepository
    {
        public MarketScheduleRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<List<MarketSchedule>> GetScheduleDaysInRangeAsync(DateTime? minDate, DateTime? maxDate) =>
            await GetAsync(
                predicate: x =>
                    (minDate == null || x.Date > minDate.Value) &&
                    (maxDate == null || x.Date < maxDate.Value),
                orderBy: x => x.OrderBy(x => x.Date));

        public async Task<MarketSchedule?> GetLastestMarketDayAsync() =>
            await GetFirstOrDefaultAsync(
                predicate: x => x.Date <= DateTime.Now.Date,
                orderBy: x => x.OrderByDescending(x => x.Date));
    }
}
