using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Persistance.Database.Repositories.Logging;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Service.Logging
{
    public interface IAppLogService : ICoreService
    {
        Task<List<int>> GetLogEventIdsAsync();
        Task<PaginateDto<AppLogDto>> GetAppLogsAsync(PaginateFilterDto<AppLogFilterDto> filter);
        Task RemoveAppLogsAsync(int daysOld);
    }

    public class AppLogService : BaseCoreService, IAppLogService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AppLogService> _logger;
        private readonly IAppLogRepository _appLogRepository;

        public AppLogService(IMapper mapper,
            ILogger<AppLogService> logger,
            IAppLogRepository appLogRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _appLogRepository = appLogRepository;
        }

        public async Task<List<int>> GetLogEventIdsAsync()
        {
            try
            {
                return await _appLogRepository.AllEventIdsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PaginateDto<AppLogDto>> GetAppLogsAsync(PaginateFilterDto<AppLogFilterDto> filter)
        {
            try
            {
                return _mapper.Map<PaginateDto<AppLogDto>>(
                    await _appLogRepository.GetAllPaginatedLogsAsync(filter));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task RemoveAppLogsAsync(int daysOld)
        {
            try
            {
                var cleanDate = DateTime.UtcNow.AddDays(-1 * daysOld);
                var logs = await _appLogRepository.GetLogsBerforeDate(cleanDate);
                if (logs.Count > 0)
                {
                    _appLogRepository.RemoveRange(logs);
                    await _appLogRepository.SaveChangesAsync();
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
