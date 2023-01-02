using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITempDisabledFetchHistoryDataRepository : IRepository<TempDisabledFetchHistoryData>
    {
        Task<List<TempDisabledFetchHistoryData>> GetAll();
        Task<bool> IsFetchHistDataDisabled(int tickerId, HistoryDataIntervalEnum interval);
    }

    public class TempDisabledFetchHistoryDataRepository : BaseRepository<TempDisabledFetchHistoryData>, ITempDisabledFetchHistoryDataRepository
    {
        public TempDisabledFetchHistoryDataRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<List<TempDisabledFetchHistoryData>> GetAll() =>
            await GetListAsync();


        public async Task<bool> IsFetchHistDataDisabled(int tickerId, HistoryDataIntervalEnum interval) =>
            await GenerateQuery(
                predicate: x =>
                    x.TickerId == tickerId &&
                    x.Interval == interval)
            .CountAsync() > 0;
    }
}
