using AutoMapper;
using Marketeer.Core.Domain.Dtos.News;
using Marketeer.Core.Domain.Entities.News;
using Marketeer.Infrastructure.Python.News;
using Marketeer.Persistance.Database.Repositories.Market;
using Marketeer.Persistance.Database.Repositories.News;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Service.News
{
    public interface INewsService : ICoreService
    {
        Task<List<NewsArticleDto>> GetTickerNewsArticles(List<int> tickerIds, int limit);
        Task<List<NewsArticleDto>> GetNewFinanceNewsArticles(int limit);
        Task<List<NewsArticleDto>> GetNewTickerNewsArticles(int tickerId);
        Task<List<NewsArticleDto>> GetNewFinanceNewsArticles();
    }

    public class NewsService : BaseCoreService, INewsService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<NewsService> _logger;
        private readonly INewsPythonService _newsPythonService;
        private readonly ITickerRepository _tickerRepository;
        private readonly INewsArticleRepository _newsArticleRepository;

        public NewsService(IMapper mapper,
            ILogger<NewsService> logger,
            INewsPythonService newsPythonService,
            ITickerRepository tickerRepository,
            INewsArticleRepository newsArticleRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _newsPythonService = newsPythonService;
            _tickerRepository = tickerRepository;
            _newsArticleRepository = newsArticleRepository;
        }

        public async Task<List<NewsArticleDto>> GetTickerNewsArticles(List<int> tickerIds, int limit)
        {
            try
            {
                var news = await _newsArticleRepository.GetTickerNewsArticlesAsync(tickerIds, null, null, limit);
                return _mapper.Map<List<NewsArticleDto>>(news);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<NewsArticleDto>> GetNewFinanceNewsArticles(int limit)
        {
            try
            {
                var news = await _newsArticleRepository.GetFinanceNewsArticlesAsync(null, null, limit);
                return _mapper.Map<List<NewsArticleDto>>(news);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<NewsArticleDto>> GetNewTickerNewsArticles(int tickerId)
        {
            try
            {
                var news = new List<NewsArticle>();
                var ticker = await _tickerRepository.GetTickerByIdAsync(tickerId);
                var newNews = await _newsPythonService.GetTickerNewsArticlesAsync(ticker!.Symbol);
                var currentNews = await _newsArticleRepository.GetLatestTickerNewsArticlesAsync(tickerId, newNews.Count);
                foreach (var n in newNews)
                {
                    if (!currentNews.Any(x => x.Link == n.Link))
                    {
                        var newsArticle = _mapper.Map<NewsArticle>(n);
                        newsArticle.TickerId = ticker.Id;
                        news.Add(newsArticle);
                    }
                }

                if (news.Count > 0)
                {
                    await _newsArticleRepository.AddRangeAsync(news);
                    await _newsArticleRepository.SaveChangesAsync();
                }

                return _mapper.Map<List<NewsArticleDto>>(news);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<List<NewsArticleDto>> GetNewFinanceNewsArticles()
        {
            try
            {
                var news = new List<NewsArticle>();
                var newNews = await _newsPythonService.GetFinanceNewsArticlesAsync();
                var currentNews = await _newsArticleRepository.GetLatestFinanceNewsArticlesAsync(newNews.Count);
                foreach (var n in newNews)
                {
                    if (!currentNews.Any(x => x.Link == n.Link))
                        news.Add(_mapper.Map<NewsArticle>(n));
                }

                if (news.Count > 0)
                {
                    await _newsArticleRepository.AddRangeAsync(news);
                    await _newsArticleRepository.SaveChangesAsync();
                }

                return _mapper.Map<List<NewsArticleDto>>(news);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
