using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Persistance.Database.Repositories.Logging;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Service.Logging
{
    public interface IPythonLogService : ICoreService
    {
        Task<List<string>> GetAllFilesAsync();
        Task<PaginateDto<PythonLogDto>> GetPythonLogsAsync(PaginateFilterDto<PythonLogFilterDto> filter);
        Task RemovePythonLogsAsync(int daysOld);
    }

    public class PythonLogService : BaseCoreService, IPythonLogService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PythonLogService> _logger;
        private readonly IPythonLogRepository _pythonLogRepository;

        public PythonLogService(IMapper mapper,
            ILogger<PythonLogService> logger,
            IPythonLogRepository pythonLogRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _pythonLogRepository = pythonLogRepository;
        }

        public async Task<List<string>> GetAllFilesAsync()
        {
            try
            {
                return await _pythonLogRepository.AllFilesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PaginateDto<PythonLogDto>> GetPythonLogsAsync(PaginateFilterDto<PythonLogFilterDto> filter)
        {
            try
            {
                return _mapper.Map<PaginateDto<PythonLogDto>>(
                    await _pythonLogRepository.GetAllPaginatedLogsAsync(filter));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task RemovePythonLogsAsync(int daysOld)
        {
            try
            {
                var cleanDate = DateTime.Now.AddDays(-1 * daysOld);
                var logs = await _pythonLogRepository.GetLogsBerforeDateAsync(cleanDate);
                if (logs.Count > 0)
                {
                    _pythonLogRepository.RemoveRange(logs);
                    await _pythonLogRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
