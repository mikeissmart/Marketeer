using AutoMapper;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Service.Market;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryDataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHistoryDataService _historyDataService;
        private readonly ITickerService _tickerService;
        private readonly IMarketScheduleService _marketScheduleService;

        public HistoryDataController(IMapper mapper,
            IHistoryDataService historyDataService,
            ITickerService tickerService,
            IMarketScheduleService marketScheduleService)
        {
            _mapper = mapper;
            _historyDataService = historyDataService;
            _tickerService = tickerService;
            _marketScheduleService = marketScheduleService;
        }

        [HttpGet("GetHistoryData")]
        public async Task<IActionResult> GetHistoryData([FromQuery] string symbol, [FromQuery] HistoryDataIntervalEnum interval)
        {
            var ticker = await _tickerService.GetTickerBySymbolAsync(symbol);
            var data = await _historyDataService.GetHistoryDataAsync(ticker!.Id, interval);
                        return Ok(data);
        }

        [HttpGet("GetTickerHistorySummary")]
        public async Task<IActionResult> GetTickerHistorySummary([FromQuery] int tickerId)
        {
            var data = await _historyDataService.GetTickerHistorySummaryAsync(tickerId);
            return Ok(data);
        }

        [HttpGet("UpdateTickerHistoryData")]
        public async Task<IActionResult> UpdateTickerHistoryData([FromQuery] int tickerId)
        {
            var result = await _historyDataService.UpdateTickerHistoryDataAsync(tickerId);
            return Ok(result);
        }
    }
}
