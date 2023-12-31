using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Dtos.News;
using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Entities.News;
using Marketeer.Infrastructure.Python.News;
using Marketeer.Persistance.Database.Repositories.AI;
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
        Task<List<NewsArticleDto>> GetNewsArticlesByIdsAsync(List<int> ids);
        Task<PaginateDto<NewsArticleDto>> GetTickerNewsArticlesAsync(PaginateFilterDto<NewsFilterDto> filter);
        Task<bool> UpdateTickerNewsArticlesAsync(int tickerId);
    }

    public class NewsService : BaseCoreService, INewsService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<NewsService> _logger;
        private readonly INewsPythonService _newsPythonService;
        private readonly ITickerRepository _tickerRepository;
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IHuggingFaceModelRepository _huggingFaceModelRepository;

        public NewsService(IMapper mapper,
            ILogger<NewsService> logger,
            INewsPythonService newsPythonService,
            ITickerRepository tickerRepository,
            INewsArticleRepository newsArticleRepository,
            IHuggingFaceModelRepository huggingFaceModelRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _newsPythonService = newsPythonService;
            _tickerRepository = tickerRepository;
            _newsArticleRepository = newsArticleRepository;
            _huggingFaceModelRepository = huggingFaceModelRepository;
        }

        public async Task<List<NewsArticleDto>> GetNewsArticlesByIdsAsync(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<NewsArticleDto>>(await _newsArticleRepository.GetNewsArticlesByIdsAsync(ids));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<PaginateDto<NewsArticleDto>> GetTickerNewsArticlesAsync(PaginateFilterDto<NewsFilterDto> filter)
        {
            try
            {
                return _mapper.Map<PaginateDto<NewsArticleDto>>(await _newsArticleRepository.GetTickerNewsArticlesAsync(filter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<bool> UpdateTickerNewsArticlesAsync(int tickerId)
        {
            try
            {
                var defaultSentimentModel = await _huggingFaceModelRepository.GetDefaultHuggingFaceModelAsync();
                var ticker = await _tickerRepository.GetTickerByIdAsync(tickerId, withNewsArticles: true);
                var newnews = await _newsPythonService.GetTickerNewsArticlesLinksAsync(ticker!.Symbol);

                var needTexts = await _newsArticleRepository.CalculateNotExistingLinksAsync(newnews);
                var newNewsLinks = newnews
                    .Select(x => x.Link)
                    .ToList();
                var existingNews = await _newsArticleRepository.GetNewsArticlesByLinksAsync(tickerId, newNewsLinks);

                if (needTexts.Count > 0)
                    needTexts = await _newsPythonService.GetNewsArticlesTextAsync(needTexts);

                var news = _mapper.Map<List<NewsArticle>>(needTexts);
                if (defaultSentimentModel != null)
                {
                    foreach (var n in news)
                    {
                        n.SentimentResults = new List<SentimentResult>
                        {
                            new SentimentResult{ HuggingFaceModelId = defaultSentimentModel.Id }
                        };
                    }

                    foreach (var n in existingNews)
                    {
                        if (n.SentimentResults.Count == 0)
                            n.SentimentResults.Add(new SentimentResult { HuggingFaceModelId = defaultSentimentModel.Id });
                    }
                }

                ticker.NewsArticles.AddRange(news);
                ticker.NewsArticles.AddRange(existingNews);

                ticker.LastNewsUpdateDateTime = DateTime.Now;
                _tickerRepository.Update(ticker);
                await _tickerRepository.SaveChangesAsync();

                return news.Count > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
