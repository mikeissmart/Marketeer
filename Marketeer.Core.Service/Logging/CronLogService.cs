using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Persistance.Database.Repositories.Logging;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Service.Logging
{
    public interface ICronLogService : ICoreService
    {
        Task<List<string>> GetAllNamesAsync();
        Task<PaginateDto<CronLogDto>> GetCronLogsAsync(PaginateFilterDto<CronLogFilterDto> filter);
        Task RemoveCronLogsAsync(int daysOld);
    }

    public class CronLogService : BaseCoreService, ICronLogService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CronLogService> _logger;
        private readonly ICronLogRepository _cronLogRepository;

        public CronLogService(IMapper mapper,
            ILogger<CronLogService> logger,
            ICronLogRepository cronLogRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _cronLogRepository = cronLogRepository;
        }

        public async Task<List<string>> GetAllNamesAsync()
        {
            try
            {
                return await _cronLogRepository.AllNamesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PaginateDto<CronLogDto>> GetCronLogsAsync(PaginateFilterDto<CronLogFilterDto> filter)
        {
            try
            {
                return _mapper.Map<PaginateDto<CronLogDto>>(
                    await _cronLogRepository.GetAllPaginatedLogsAsync(filter));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task RemoveCronLogsAsync(int daysOld)
        {
            try
            {
                var cleanDate = DateTime.UtcNow.AddDays(-1 * daysOld);
                var logs = await _cronLogRepository.GetLogsBerforeDateAsync(cleanDate);
                if (logs.Count > 0)
                {
                    _cronLogRepository.RemoveRange(logs);
                    await _cronLogRepository.SaveChangesAsync();
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
