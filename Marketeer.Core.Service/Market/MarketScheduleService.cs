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
        Task GetYearlyMarketSchedulesAsync(int numYears);
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

        public async Task GetYearlyMarketSchedulesAsync(int numYears)
        {
            try
            {
                for (var i = 0; i < numYears; i++)
                {
                    var minDate = new DateTime(DateTime.Now.Year + i, 1, 1);
                    var maxDate = new DateTime(DateTime.Now.Year + i, 12, 31);

                    var curSchedules = await _marketScheduleRepository.GetScheduleDaysInRangeAsync(minDate, maxDate);

                    var freshSchedules = _mapper.Map<List<MarketSchedule>>(await _marketPythonService.GetYearlyMarketSchedule(minDate.Year));

                    var addSchedules = freshSchedules
                        .Where(x => !curSchedules.Any(y =>
                            y.Day == x.Day &&
                            y.MarketOpen == x.MarketOpen &&
                            y.MarketClose == x.MarketClose));
                    var removeSchedules = curSchedules
                        .Where(x => !freshSchedules.Any(y =>
                            y.Day == x.Day &&
                            y.MarketOpen == x.MarketOpen &&
                            y.MarketClose == x.MarketClose));

                    await _marketScheduleRepository.AddRangeAsync(addSchedules);
                    _marketScheduleRepository.RemoveRange(removeSchedules);

                    await _marketScheduleRepository.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
