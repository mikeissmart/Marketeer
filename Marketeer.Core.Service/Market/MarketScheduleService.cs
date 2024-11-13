using AutoMapper;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Infrastructure.Python.Market;
using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Service.Market
{
    public interface IMarketScheduleService : ICoreService
    {
        Task GetYearlyMarketSchedulesAsync(int minYear, int numOfYears);
    }

    public class MarketScheduleService : BaseCoreService, IMarketScheduleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TickerService> _logger;
        private readonly IMarketScheduleRepository _marketScheduleRepository;
        private readonly IMarketPythonService _marketPythonService;

        public MarketScheduleService(IMapper mapper,
            ILogger<TickerService> logger,
            IMarketScheduleRepository marketScheduleRepository,
            IMarketPythonService marketPythonService)
        {
            _mapper = mapper;
            _logger = logger;
            _marketScheduleRepository = marketScheduleRepository;
            _marketPythonService = marketPythonService;
        }

        public async Task GetYearlyMarketSchedulesAsync(int minYear, int numOfYears)
        {
            try
            {
                // +1 for this year
                for (var i = 0; i < numOfYears + 1; i++)
                {
                    var minDate = new DateTime(minYear + i - 1, 12, 31);
                    var maxDate = new DateTime(minYear + i + 1, 1, 1);

                    var curSchedules = await _marketScheduleRepository.GetScheduleDaysInRangeAsync(minDate, maxDate);

                    var freshSchedules = _mapper.Map<List<MarketSchedule>>(await _marketPythonService.GetYearlyMarketSchedule(minYear + i));
                    var addSchedules = freshSchedules
                        .Where(x => !curSchedules.Any(y => y.Date == x.Date));
                    var removeSchedules = curSchedules
                        .Where(x => !freshSchedules.Any(y => y.Date == x.Date));

                    if (addSchedules.Count() > 0)
                        await _marketScheduleRepository.AddRangeAsync(addSchedules);
                    if (removeSchedules.Count() > 0)
                        _marketScheduleRepository.RemoveRange(removeSchedules);

                    await _marketScheduleRepository.SaveChangesAsync();
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
