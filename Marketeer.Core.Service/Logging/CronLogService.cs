using AutoMapper;
using Marketeer.Core.Domain.Dtos.Logging;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.Repositories.Logging;

namespace Marketeer.Core.Service.Logging
{
    public interface ICronLogService : IService
    {
        Task AddCronLogAsync(CronLogDto cronLogDto);
        Task RemoveCronLogsAsync(int daysOld);
    }

    public class CronLogService : BaseService, ICronLogService
    {
        private readonly ICronLogRepository _cronLogRepository;
        private readonly IMapper _mapper;

        public CronLogService(ICronLogRepository cronLogRepository, IMapper mapper)
        {
            _cronLogRepository = cronLogRepository;
            _mapper = mapper;
        }

        public async Task AddCronLogAsync(CronLogDto cronLogDto)
        {
            var cronLog = _mapper.Map<CronLog>(cronLogDto);
            await _cronLogRepository.AddAsync(cronLog);
            await _cronLogRepository.SaveChangesAsync();
        }

        public async Task RemoveCronLogsAsync(int daysOld)
        {
            var cleanDate = DateTime.UtcNow.AddDays(-1 * daysOld);
            var logs = await _cronLogRepository.GetListAsync<CronLog>(x => x.StartDate < cleanDate);
            if (logs.Count > 0)
            {
                _cronLogRepository.RemoveRange(logs);
                await _cronLogRepository.SaveChangesAsync();
            }
        }
    }
}
