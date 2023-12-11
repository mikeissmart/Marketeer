using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Service.ChronJob;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CronJobController : ControllerBase
    {
        private readonly ICronJobService _cronJobService;

        public CronJobController(ICronJobService cronJobService)
        {
            _cronJobService = cronJobService;
        }

        [HttpPost("GetCronJob")]
        public async Task<IActionResult> GetCronJob([FromBody] PaginateFilterDto filter)
        {
            var data = await _cronJobService.GetChronJobsAsync(filter);
            return Ok(data);
        }

        [HttpGet("FireCronJob")]
        public async Task<IActionResult> FireCronJob([FromQuery] string cronJobName)
        {
            var data = await _cronJobService.FireCronJobAsync(cronJobName);
            return Ok(data);
        }
    }
}
