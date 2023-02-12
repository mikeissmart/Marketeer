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
    public interface IJsonTickerInfoRepository : IRepository<JsonTickerInfo>
    {
        Task<JsonTickerInfo?> GetJsonTickerBySymbolAsync(string symbol);
        Task<List<string>> GetJsonTickerBySymbolsAsync(List<string> symbols);
    }

    public class JsonTickerInfoRepository : BaseRepository<JsonTickerInfo>, IJsonTickerInfoRepository
    {
        public JsonTickerInfoRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<JsonTickerInfo?> GetJsonTickerBySymbolAsync(string symbol) =>
            await GetFirstOrDefaultAsync(x => x.Symbol == symbol);

        public async Task<List<string>> GetJsonTickerBySymbolsAsync(List<string> symbols) =>
            await GenerateQuery(x => symbols.Contains(x.Symbol))
                .Select(x => x.Symbol)
                .ToListAsync();
    }
}
