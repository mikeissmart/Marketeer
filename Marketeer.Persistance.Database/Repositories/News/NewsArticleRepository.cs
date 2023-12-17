using Marketeer.Core.Domain.Dtos.News;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Entities.News;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain;

namespace Marketeer.Persistance.Database.Repositories.News
{
    public interface INewsArticleRepository : IRepository<NewsArticle>
    {
        Task<Paginate<NewsArticle>> GetTickerNewsArticlesAsync(int tickerId, PaginateFilterDto<NewsFilterDto> filter);
        Task<List<NewsArticleDto>> CalculateNotExistingLinksAsync(List<NewsArticleDto> newNews);
        Task<List<NewsArticle>> GetNewsArticlesByLinksAsync(int withoutTickerId, List<string> newsLinks);
    }

    public class NewsArticleRepository : BaseRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<Paginate<NewsArticle>> GetTickerNewsArticlesAsync(int tickerId, PaginateFilterDto<NewsFilterDto> filter) =>
            await GetPaginateAsync(
                filter,
                predicate: x =>
                    (x.Tickers.Any(x => x.Id == tickerId)) &&
                    (filter.Filter.MinDate == null || x.Date.Date > filter.Filter.MinDate.Value.Date) &&
                    (filter.Filter.MaxDate == null || x.Date.Date <= filter.Filter.MaxDate.Value.Date),
                orderBy: CalculateOrderBy(filter));

        public async Task<List<NewsArticleDto>> CalculateNotExistingLinksAsync(List<NewsArticleDto> newNews)
        {
            var existingLinks = await GenerateQuery()
                .Select(x => x.Link)
                .ToListAsync();
            return newNews
                .Where(x => !existingLinks.Contains(x.Link))
                .ToList();
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByLinksAsync(int withoutTickerId, List<string> newsLinks) =>
            await GetAsync(
                predicate: x => newsLinks.Contains(x.Link) && !x.Tickers.Any(x => x.Id == withoutTickerId),
                include: x => x
                    .Include(x => x.Tickers));

        private Func<IQueryable<NewsArticle>, IOrderedQueryable<NewsArticle>>? CalculateOrderBy(PaginateFilterDto filter)
        {
            Func<IQueryable<NewsArticle>, IOrderedQueryable<NewsArticle>>? orderBy = null;
            switch (filter.OrderBy)
            {
                case nameof(NewsArticle.Date):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Date)
                        : x => x.OrderByDescending(x => x.Date);
                    break;
                case nameof(NewsArticle.Title):
                    orderBy = filter.IsOrderAsc
                        ? x => x.OrderBy(x => x.Title)
                        : x => x.OrderByDescending(x => x.Title);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return orderBy;
        }
    }
}
