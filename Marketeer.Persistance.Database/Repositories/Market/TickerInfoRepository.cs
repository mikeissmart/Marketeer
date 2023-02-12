using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Marketeer.Persistance.Database.Repositories.Market
{
    public interface ITickerInfoRepository : IRepository<TickerInfo>
    {
        Task<int> ListedTickerCountAsync();
        Task<List<TickerInfo>> GetOldestListedTickerInfosAsync(int limitCount);
        Task<List<string>> SearchNamesAsync(string? search, int limit);
        Task<List<string>> SearchQuoteTypesAsync(string? search, int limit);
        Task<List<string>> SearchSectorsAsync(string? search, int limit);
        Task<List<string>> SearchIndustriesAsync(string? search, int limit);
    }

    public class TickerInfoRepository : BaseRepository<TickerInfo>, ITickerInfoRepository
    {
        public TickerInfoRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<int> ListedTickerCountAsync() =>
            await GenerateQuery(x => !x.Ticker.TickerSetting.IsDelisted).CountAsync();

        public async Task<List<TickerInfo>> GetOldestListedTickerInfosAsync(int limitCount)
        {
            return await GenerateQuery(
                    predicate: x => !x.Ticker.TickerSetting.IsDelisted,
                    include: x => x
                        .Include(x => x.Ticker)
                            .ThenInclude(x => x.TickerSetting),
                    orderBy: x => x.OrderBy(x => x.UpdatedDate),
                    tracking: false)
                .Take(limitCount)
                .ToListAsync();
        }

        public async Task<List<string>> SearchNamesAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.Name.Length > 0 &&
                (search == null || x.Name.Contains(search)))
            .Select(x => x.Name)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<List<string>> SearchQuoteTypesAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.QuoteType.Length > 0 &&
                (search == null || x.QuoteType.Contains(search)))
            .Select(x => x.QuoteType)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<List<string>> SearchSectorsAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.Sector != null &&
                x.Sector.Length > 0 &&
                (search == null || x.Sector.Contains(search)))
            .Select(x => x.Sector!)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();

        public async Task<List<string>> SearchIndustriesAsync(string? search, int limit) =>
            await GenerateQuery(x =>
                x.Industry != null &&
                x.Industry.Length > 0 &&
                (search == null || x.Industry.Contains(search)))
            .Select(x => x.Industry!)
            .Distinct()
            .OrderBy(x => x)
            .Take(limit)
            .ToListAsync();
    }
}
