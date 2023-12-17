using AutoMapper;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Service.Market;
using Microsoft.AspNetCore.Authorization;
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

        public HistoryDataController(IMapper mapper,
            IHistoryDataService historyDataService,
            ITickerService tickerService)
        {
            _mapper = mapper;
            _historyDataService = historyDataService;
            _tickerService = tickerService;
        }

        [HttpGet("GetHistoryData")]
        public async Task<IActionResult> GetHistoryData([FromQuery] int tickerId, [FromQuery] HistoryDataIntervalEnum interval)
        {
            var data = await _historyDataService.GetHistoryDataAsync(tickerId, interval);
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
