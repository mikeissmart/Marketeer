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
    }

    public class MarketScheduleRepository : BaseRepository<MarketSchedule>, IMarketScheduleRepository
    {
        public MarketScheduleRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<List<MarketSchedule>> GetScheduleDaysInRangeAsync(DateTime? minDate, DateTime? maxDate) =>
            await GetAsync(
                predicate: x =>
                    (minDate == null || x.Day.Date >= minDate.Value.Date) &&
                    (maxDate == null || x.Day.Date <= maxDate.Value.Date),
                orderBy: x => x.OrderBy(x => x.MarketOpen));
    }
}
