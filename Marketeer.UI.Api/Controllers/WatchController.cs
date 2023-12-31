using AutoMapper;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Watch;
using Marketeer.Core.Service.Market;
using Marketeer.Core.Service.Watch;
using Marketeer.UI.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WatchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWatchTickerService _watchTickerService;
        private readonly ITickerService _tickerService;

        public WatchController(IMapper mapper, IWatchTickerService WatchTickerService, ITickerService tickerService)
        {
            _mapper = mapper;
            _watchTickerService = WatchTickerService;
            _tickerService = tickerService;
        }

        [HttpGet("GetWatchTickerUpdateDaily")]
        public async Task<IActionResult> GetWatchTickerUpdateDaily([FromQuery] int tickerId)
        {
            var result = await _watchTickerService.GetWatchTickerUpdateDailyAsync(tickerId, User.GetUserId());
            return Ok(result);
        }

        [HttpPost("UpdateWatchTickerUpdateDaily")]
        public async Task<IActionResult> UpdateWatchTickerUpdateDaily([FromBody] WatchTickerChangeDto changeDto)
        {
            var result = await _watchTickerService.UpdateWatchTickerUpdateDailyAsync(changeDto, User.GetUserId());
            return Ok(result);
        }

        [HttpGet("GetWatcherUserStatus")]
        public async Task<IActionResult> GetWatcherUserStatus()
        {
            var result = await _watchTickerService.GetWatcherUserStatusAsync(User.GetUserId());
            return Ok(result);
        }

        [HttpPost("AppendWatchTickerDetails")]
        public async Task<IActionResult> AppendWatchTickerDetails([FromBody] WatchTickerDetailsChangeDto changeFilterDto)
        {
            changeFilterDto.Filter.IsPaginated = false;
            changeFilterDto.Filter.Filter.IsListed = true;
            var tickers = await _tickerService.GetTickerDetailsAsync(changeFilterDto.Filter, User.GetUserId());
            var changeDto = new WatchTickerChangeDto
            {
                TickerIds = tickers.Items.Select(x => x.Id).ToList(),
                UpdateHistoryData = changeFilterDto.UpdateHistoryData,
                UpdateNewsArticles = changeFilterDto.UpdateNewsArticles
            };
            var result = await _watchTickerService.UpdateWatchTickerUpdateDailyAsync(changeDto, User.GetUserId());
            return Ok(result);
        }
    }
}
