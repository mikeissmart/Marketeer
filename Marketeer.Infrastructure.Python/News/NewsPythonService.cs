using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.News;
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
        Task<List<NewsArticleDto>> GetTickerNewsArticlesAsync(string symbol);
        Task<List<NewsArticleDto>> GetFinanceNewsArticlesAsync();
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

        public async Task<List<NewsArticleDto>> GetTickerNewsArticlesAsync(string symbol)
        {
            var args = new FinvizFinanceNewsArgs
            {
                Symbol = symbol
            };
            var results = await RunPythonScriptAsync<List<FinvizFinanceNewsDto>, FinvizFinanceNewsArgs>(
                _newsPythonConfig.FinvizFinanceNews, args);
            var dtos = _mapper.Map<List<NewsArticleDto>>(results);

            return dtos;
        }

        public async Task<List<NewsArticleDto>> GetFinanceNewsArticlesAsync()
        {
            var args = new FinvizFinanceNewsArgs();
            var results = await RunPythonScriptAsync<List<FinvizFinanceNewsDto>, FinvizFinanceNewsArgs>(
                _newsPythonConfig.FinvizFinanceNews, args);
            var dtos = _mapper.Map<List<NewsArticleDto>>(results);

            return dtos;
        }
    }
}
