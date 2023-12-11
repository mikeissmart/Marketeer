using AutoMapper;
using Cronos;
using Marketeer.Core.CronJob;
using Marketeer.Core.Domain.Domain.CronJob;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.CronJob;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.Repositories.CronJob;
using Marketeer.Persistance.Database.Repositories.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Service.ChronJob
{
    public interface ICronJobService
    {
        Task<PaginateDto<CronJobDetailDto>> GetChronJobsAsync(PaginateFilterDto filter);
        Task<bool> FireCronJobAsync(string name);
    }

    public class CronJobService : ICronJobService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CronJobService> _logger;
        private readonly IServiceProvider _services;
        private readonly ICronJobStatusRepository _cronJobStatusRepository;
        private readonly ICronLogRepository _cronLogRepository;

        public CronJobService(IMapper mapper, ILogger<CronJobService> logger, IServiceProvider services, ICronJobStatusRepository cronJobStatusRepository, ICronLogRepository cronLogRepository = null)
        {
            _mapper = mapper;
            _logger = logger;
            _services = services;
            _cronJobStatusRepository = cronJobStatusRepository;
            _cronLogRepository = cronLogRepository;
        }

        public async Task<PaginateDto<CronJobDetailDto>> GetChronJobsAsync(PaginateFilterDto filter)
        {
            try
            {
                var dtos = _mapper.Map<List<CronJobDetailDto>>(GetCronJobDetails());
                var statuses = await _cronJobStatusRepository.GetAllAsync();
                var logs = await _cronLogRepository.GetLastLogForCronJobsAsync(dtos.Select(x => x.Name).ToList());
                foreach (var dto in dtos)
                {
                    var status = statuses.FirstOrDefault(x => x.Name == dto.Name);
                    dto.IsRunning = statuses
                        .FirstOrDefault(x => x.Name == dto.Name)
                        ?.IsRunning ?? false;
                    dto.LastOccurrence = logs
                        .FirstOrDefault(x => x.Name == dto.Name)
                        ?.StartDate;
                }

                switch (filter.OrderBy)
                {
                    case nameof(CronJobDetailDto.Name):
                        dtos = filter.IsOrderAsc
                            ? dtos.OrderBy(x => x.Name).ToList()
                            : dtos.OrderByDescending(x => x.Name).ToList();
                        break;
                    case nameof(CronJobDetailDto.NextOccurrence):
                        dtos = filter.IsOrderAsc
                            ? dtos.OrderBy(x => x.NextOccurrence).ToList()
                            : dtos.OrderByDescending(x => x.NextOccurrence).ToList();
                        break;
                    case nameof(CronJobDetailDto.LastOccurrence):
                        dtos = filter.IsOrderAsc
                            ? dtos.OrderBy(x => x.LastOccurrence).ToList()
                            : dtos.OrderByDescending(x => x.LastOccurrence).ToList();
                        break;
                }

                return new PaginateDto<CronJobDetailDto>
                {
                    PageItemCount = dtos.Count,
                    Items = dtos,
                    TotalPages = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> FireCronJobAsync(string name)
        {
            try
            {
                var detail = GetCronJobDetails()
                    .FirstOrDefault(x => x.Name == name);

                if (detail == null)
                    return false;

                var job = (BaseCronJobService)_services.GetRequiredService(detail.CronJobType);
                job.DoWorkNow();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private List<CronJobDetail> GetCronJobDetails()
        {
            var cronType = typeof(BaseCronJobService);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => cronType.IsAssignableFrom(x) &&
                    !x.IsAbstract &&
                    x.IsClass &&
                    x != cronType);

            var dtos = new List<CronJobDetail>();
            foreach (var type in types)
            {
                var job = (BaseCronJobService)_services.GetRequiredService(type);
                dtos.Add(new CronJobDetail
                {
                    CronJobType = type,
                    Name = job.GetType().Name,
                    CronExpression = job.GetCronExpression(),
                    NextOccurrence = job.NextOccurrence()?.UtcDateTime
                });
            }

            return dtos;
        }
    }
}
