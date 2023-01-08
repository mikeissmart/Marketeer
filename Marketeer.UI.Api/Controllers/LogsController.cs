using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Core.Service.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketeer.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly IAppLogService _appLogService;
        private readonly ICronLogService _cronLogService;
        private readonly IPythonLogService _pythonLogService;

        public LogsController(
            IAppLogService appLogService,
            ICronLogService cronLogService,
            IPythonLogService pythonLogService)
        {
            _appLogService = appLogService;
            _cronLogService = cronLogService;
            _pythonLogService = pythonLogService;
        }

        [HttpGet("GetAppLogEventIds")]
        public async Task<IActionResult> GetAppLogEventIds()
        {
            var data = await _appLogService.GetLogEventIdsAsync();
            return Ok(data);
        }

        [HttpPost("GetAppLogs")]
        public async Task<IActionResult> GetAppLogs([FromBody] PaginateFilterDto<AppLogFilterDto> filter)
        {
            var data = await _appLogService.GetAppLogsAsync(filter);
            return Ok(data);
        }

        [HttpGet("GetCronNames")]
        public async Task<IActionResult> GetCronNames()
        {
            var data = await _cronLogService.GetAllNamesAsync();
            return Ok(data);
        }

        [HttpPost("GetCronLogs")]
        public async Task<IActionResult> GetCronLogs([FromBody] PaginateFilterDto<CronLogFilterDto> filter)
        {
            var data = await _cronLogService.GetCronLogsAsync(filter);
            return Ok(data);
        }

        [HttpPost("GetPythonLogs")]
        public async Task<IActionResult> GetPythonLogs([FromBody] PaginateFilterDto<PythonLogFilterDto> filter)
        {
            var data = await _pythonLogService.GetPythonLogsAsync(filter);
            return Ok(data);
        }

        [HttpGet("GetPythonFiles")]
        public async Task<IActionResult> GetPythonFiles()
        {
            var data = await _pythonLogService.GetAllFilesAsync();
            return Ok(data);
        }
    }
}
