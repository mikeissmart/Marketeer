using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Core.Domain.Enums;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.AI
{
    public interface ISentimentResultRepository : IRepository<SentimentResult>
    {
        Task<List<SentimentResult>> GetLatestQueuedSentimentsAsync(int limit, bool tracking = true);
        Task<List<SentimentResult>> GetQueuedSentimentsAsync();
    }

    public class SentimentResultRepository : BaseRepository<SentimentResult>, ISentimentResultRepository
    {
        public SentimentResultRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }

        public async Task<List<SentimentResult>> GetLatestQueuedSentimentsAsync(int limit, bool tracking = true) =>
            await GetAsync(
                predicate: x => x.Status != SentimentStatusEnum.Completed,
                include: x => x
                    .Include(x => x.NewsArticle),
                orderBy: x => x.OrderBy(x => x.UpdatedDateTime != null
                    ? x.UpdatedDateTime
                    : x.CreatedDateTime),
                take: limit,
                tracking: tracking);

        public async Task<List<SentimentResult>> GetQueuedSentimentsAsync() =>
            await GetAsync(
                predicate: x => x.Status == SentimentStatusEnum.Queued);
    }
}
