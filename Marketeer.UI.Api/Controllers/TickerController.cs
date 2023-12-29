using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Service.Market;
using Marketeer.UI.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("GetTickerById")]
        public async Task<IActionResult> GetTickerById([FromQuery] int tickerId)
        {
            var result = await _tickerService.GetTickerByIdAsync(tickerId);
            return Ok(result);
        }

        [HttpGet("GetTickerBySymbol")]
        public async Task<IActionResult> GetTickerBySymbol([FromQuery] string symbol)
        {
            var result = await _tickerService.GetTickerBySymbolAsync(symbol);
            return Ok(result);
        }

        [HttpGet("SearchNames")]
        public async Task<IActionResult> SearchNames(string? search, int limit)
        {
            var result = await _tickerService.SearchNamesAsync(search, limit);
            return Ok(result);
        }

        [HttpGet("SearchSymbols")]
        public async Task<IActionResult> SearchSymbols(string? search, int limit)
        {
            var result = await _tickerService.SearchSymbolsAsync(search, limit);
            return Ok(result);
        }

        [HttpGet("SearchQuoteTypes")]
        public async Task<IActionResult> SearchQuoteTypes(string? search, int limit)
        {
            var result = await _tickerService.SearchQuoteTypesAsync(search, limit);
            return Ok(result);
        }

        [HttpGet("SearchSectors")]
        public async Task<IActionResult> SearchSectors(string? search, int limit)
        {
            var result = await _tickerService.SearchSectorsAsync(search, limit);
            return Ok(result);
        }

        [HttpGet("SearchIndustries")]
        public async Task<IActionResult> SearchIndustries(string? search, int limit)
        {
            var result = await _tickerService.SearchIndustriesAsync(search, limit);
            return Ok(result);
        }

        [HttpPost("GetTickerDetails")]
        public async Task<IActionResult> GetTickerDetails(PaginateFilterDto<TickerFilterDto> filter)
        {
            var result = await _tickerService.GetTickerDetailsAsync(filter, User.GetUserId());
            return Ok(result);
        }

        [HttpGet("UpdateTickerInfoData")]
        public async Task<IActionResult> UpdateTickerInfoData([FromQuery] int tickerId)
        {
            var result = await _tickerService.UpdateTickerInfoAsync(tickerId);
            return Ok(result);
        }
    }
}
