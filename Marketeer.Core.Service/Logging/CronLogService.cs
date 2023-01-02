using AutoMapper;
using Marketeer.Persistance.Database.Repositories.Logging;

namespace Marketeer.Core.Service.Logging
{
    public interface ICronLogService : ICoreService
    {
        Task RemoveCronLogsAsync(int daysOld);
    }

    public class CronLogService : BaseCoreService, ICronLogService
    {
        private readonly ICronLogRepository _cronLogRepository;
        private readonly IMapper _mapper;

        public CronLogService(ICronLogRepository cronLogRepository, IMapper mapper)
        {
            _cronLogRepository = cronLogRepository;
            _mapper = mapper;
        }

        public async Task RemoveCronLogsAsync(int daysOld)
        {
            var cleanDate = DateTime.UtcNow.AddDays(-1 * daysOld);
            var logs = await _cronLogRepository.GetLogsBerforeStartDateAsync(cleanDate);
            if (logs.Count > 0)
            {
                _cronLogRepository.RemoveRange(logs);
                await _cronLogRepository.SaveChangesAsync();
            }
        }
    }
}
