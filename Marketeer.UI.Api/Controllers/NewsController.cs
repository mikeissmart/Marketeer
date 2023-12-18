using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.News;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Service.Market;
using Marketeer.Core.Service.News;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INewsService _newsService;

        public NewsController(IMapper mapper,
            INewsService newsService)
        {
            _mapper = mapper;
            _newsService = newsService;
        }

        [HttpPost("GetNews")]
        public async Task<IActionResult> GetTickerNews([FromBody] PaginateFilterDto<NewsFilterDto> filter)
        {
            var data = await _newsService.GetTickerNewsArticlesAsync(filter);
            return Ok(data);
        }

        [HttpGet("UpdateTickerNews")]
        public async Task<IActionResult> UpdateTickerNews([FromQuery] int tickerId)
        {
            var result = await _newsService.UpdateTickerNewsArticlesAsync(tickerId);
            return Ok(result);
        }
    }
}
