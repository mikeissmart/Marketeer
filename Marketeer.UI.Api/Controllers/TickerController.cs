using AutoMapper;
using Marketeer.Core.Service.Market;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TickerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITickerService _tickerService;

        public TickerController(IMapper mapper,
            ITickerService tickerService)
        {
            _mapper = mapper;
            _tickerService = tickerService;
        }

        [HttpGet("SearchSymbol")]
        public async Task<IActionResult> SearchSymbol([FromQuery] string search, [FromQuery] int limit = 10)
        {
            var result = await _tickerService.SearchSymbolAsync(search, limit);
            return Ok(result);
        }

        // TODO remove, chron only?
        [HttpGet("RefreshTickers")]
        public async Task<IActionResult> RefreshTickers()
        {
            await _tickerService.RefreshTickersAsync();
            return Ok();
        }
    }
}
