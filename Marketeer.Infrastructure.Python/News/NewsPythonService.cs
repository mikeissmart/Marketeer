using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.News;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.InfrastructureDtos.Python.News;
using Marketeer.Persistance.Database.Repositories.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Infrastructure.Python.News
{
    public interface INewsPythonService : IPythonService
    {
        Task<List<NewsArticleDto>> GetTickerNewsArticlesLinksAsync(string symbol);
        Task<List<NewsArticleDto>> GetNewsArticlesTextAsync(List<NewsArticleDto> newsLinks);
    }

    public class NewsPythonService : BasePythonService, INewsPythonService
    {
        private readonly IMapper _mapper;
        private readonly NewsPythonConfig _newsPythonConfig;

        public NewsPythonService(IMapper mapper,
            RootPythonConfig rootPythonConfig,
            NewsPythonConfig newsPythonConfig,
            IPythonLogRepository pythonLogRepository) : base(rootPythonConfig, newsPythonConfig, pythonLogRepository)
        {
            _mapper = mapper;
            _newsPythonConfig = newsPythonConfig;
        }

        public async Task<List<NewsArticleDto>> GetTickerNewsArticlesLinksAsync(string symbol)
        {
            var args = new FinvizFinanceNewsLinkArgs
            {
                Symbol = symbol
            };
            var results = await RunPythonScriptAsync<List<FinvizFinanceNewsLinkDto>, FinvizFinanceNewsLinkArgs>(
                _newsPythonConfig.FinvizFinanceNewsLink, args);

            var newsResults = new List<NewsArticleDto>();
            foreach (var dto in results)
            {
                var item = new NewsArticleDto();

                item.Title = dto.Title;
                item.Link = dto.Link;
                item.ArticleDate = dto.Date;

                newsResults.Add(item);
            }

            return newsResults;
        }

        public async Task<List<NewsArticleDto>> GetNewsArticlesTextAsync(List<NewsArticleDto> newsLinks)
        {
            var args = new FinvizFinanceNewsTextArgs
            {
                Links = newsLinks.Select(x => x.Link).ToList()
            };
            var results = await RunPythonScriptAsync<List<FinvizFinanceNewsTextDto>, FinvizFinanceNewsTextArgs>(
                _newsPythonConfig.FinvizFinanceNewsText, args);

            var newsResults = new List<NewsArticleDto>();
            foreach (var dto in results)
            {
                var item = newsLinks.First(x => x.Link == dto.Link);

                item.Text = dto.Text;

                newsResults.Add(item);
            }

            return newsResults;
        }
    }
}
