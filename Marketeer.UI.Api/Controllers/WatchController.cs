using AutoMapper;
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

        public WatchController(IMapper mapper, IWatchTickerService WatchTickerService)
        {
            _mapper = mapper;
            _watchTickerService = WatchTickerService;
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
    }
}
