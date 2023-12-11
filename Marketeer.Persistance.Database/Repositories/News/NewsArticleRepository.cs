using Marketeer.Core.Domain.Entities.News;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.Repositories.News
{
    public interface INewsArticleRepository : IRepository<NewsArticle>
    {
        Task<List<NewsArticle>> GetFinanceNewsArticlesAsync(DateTime? min, DateTime? max, int limit);
        Task<List<NewsArticle>> GetTickerNewsArticlesAsync(List<int> tickerIds, DateTime? min, DateTime? max, int limit);
        Task<bool> HasNewsArticleByLinkAsync(string link);
        Task<List<NewsArticle>> GetLatestFinanceNewsArticlesAsync(int limit);
        Task<List<NewsArticle>> GetLatestTickerNewsArticlesAsync(int tickerId, int limit);
    }

    public class NewsArticleRepository : BaseRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<List<NewsArticle>> GetFinanceNewsArticlesAsync(DateTime? min, DateTime? max, int limit) =>
            (await GetAsync(x =>
                (x.TickerId == null) &&
                (min == null || x.Date.Date >= min.Value.Date) &&
                (max == null || x.Date.Date <= max.Value.Date),
                orderBy: x => x.OrderBy(x => x.Date)))
            .Take(limit)
            .ToList();

        public async Task<List<NewsArticle>> GetTickerNewsArticlesAsync(List<int> tickerIds, DateTime? min, DateTime? max, int limit) =>
            (await GetAsync(x =>
                (x.TickerId != null && tickerIds.Contains(x.TickerId.Value)) &&
                (min == null || x.Date.Date >= min.Value.Date) &&
                (max == null || x.Date.Date <= max.Value.Date),
                orderBy: x => x.OrderBy(x => x.Date)))
            .Take(limit)
            .ToList();

        public async Task<bool> HasNewsArticleByLinkAsync(string link) =>
            await GenerateQuery(x => x.Link == link).CountAsync() > 0;

        public async Task<List<NewsArticle>> GetLatestFinanceNewsArticlesAsync(int limit) =>
            (await GetAsync(x =>
                (x.TickerId == null),
                orderBy: x => x.OrderBy(x => x.Date)))
            .Take(limit)
            .ToList();

        public async Task<List<NewsArticle>> GetLatestTickerNewsArticlesAsync(int tickerId, int limit) =>
            (await GetAsync(x =>
                (x.TickerId != null && tickerId == x.TickerId.Value),
                orderBy: x => x.OrderBy(x => x.Date)))
            .Take(limit)
            .ToList();
    }
}
