using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Service.Chron;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CronController : ControllerBase
    {
        private readonly ICronService _cronService;

        public CronController(ICronService cronJobService)
        {
            _cronService = cronJobService;
        }

        [HttpPost("GetCronJobs")]
        public async Task<IActionResult> GetCronJobs([FromBody] PaginateFilterDto filter)
        {
            var data = await _cronService.GetChronJobsAsync(filter);
            return Ok(data);
        }

        [HttpGet("FireCronJob")]
        public async Task<IActionResult> FireCronJob([FromQuery] string name)
        {
            var data = await _cronService.FireCronJobAsync(name);
            return Ok(data);
        }

        [HttpPost("GetCronQueues")]
        public async Task<IActionResult> GetCronQueuesAsync([FromBody] PaginateFilterDto filter)
        {
            var data = await _cronService.GetCronQueuesAsync(filter);
            return Ok(data);
        }

        [HttpGet("StartCronQueue")]
        public async Task<IActionResult> StartCronQueue([FromQuery] string name)
        {
            var data = await _cronService.StartCronQueueAsync(name);
            return Ok(data);
        }

        [HttpGet("StopCronQueue")]
        public async Task<IActionResult> StopCronQueue([FromQuery] string name)
        {
            var data = await _cronService.StopCronQueueAsync(name);
            return Ok(data);
        }
    }
}
