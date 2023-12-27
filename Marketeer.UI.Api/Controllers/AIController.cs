using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.AI;
using Marketeer.Core.Domain.Dtos.News;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Service.AI;
using Marketeer.Core.Service.News;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly ISentimentService _sentimentService;
        private readonly INewsService _newsService;
        private readonly IHuggingFaceService _huggingFaceService;

        public AIController(ISentimentService sentimentService, INewsService newsService, IHuggingFaceService huggingFaceService)
        {
            _sentimentService = sentimentService;
            _newsService = newsService;
            _huggingFaceService = huggingFaceService;
        }

        [HttpGet("GetHuggingFaceModels")]
        public async Task<IActionResult> GetHuggingFaceModels()
        {
            var data = await _huggingFaceService.GetHuggingFaceModelsAsync();
            return Ok(data);
        }

        [HttpPost("QueueNewsDefaultSentiment")]
        public async Task<IActionResult> QueueNewsDefaultSentiment([FromBody] List<int> ids)
        {
            var defaultHuggingFace = await _huggingFaceService.GetDefaultHuggingFaceModelAsync();
            var queue = new QueueSentimentDto
            {
                HuggingFaceModelId = defaultHuggingFace.Id,
                ItemIds = ids,
                SentimentResultType = SentimentResultTypeEnum.News_Article
            };
            var result = await _sentimentService.EnqueueSentimentAsync(queue);
            return Ok(result);
        }

        [HttpGet("QueueTickerNewsDefaultSentiment")]
        public async Task<IActionResult> QueueTickerNewsDefaultSentiment([FromQuery] string symbol)
        {
            var tickerNews = await _newsService.GetTickerNewsArticlesAsync(new PaginateFilterDto<NewsFilterDto>
            {
                Filter = new NewsFilterDto
                {
                    Symbol = symbol
                },
                IsPaginated = false
            });
            var defaultHuggingFace = await _huggingFaceService.GetDefaultHuggingFaceModelAsync();
            var queue = new QueueSentimentDto
            {
                HuggingFaceModelId = defaultHuggingFace.Id,
                ItemIds = tickerNews.Items.Select(x => x.Id).ToList(),
                SentimentResultType = SentimentResultTypeEnum.News_Article
            };
            var result = await _sentimentService.EnqueueSentimentAsync(queue);
            return Ok(result);
        }
    }
}
