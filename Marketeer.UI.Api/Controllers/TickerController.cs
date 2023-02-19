using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Market;
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
            var result = await _tickerService.GetTickerDetailsAsync(filter);
            return Ok(result);
        }
    }
}
