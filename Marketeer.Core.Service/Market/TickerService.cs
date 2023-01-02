using AutoMapper;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Persistance.Database.Repositories.Market;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Service.Market
{
    public interface ITickerService : ICoreService
    {
        Task<TickerDto> GetTickerBySymbolAsync(string symbol);
    }

    public class TickerService : BaseCoreService, ITickerService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TickerService> _logger;
        private readonly ITickerRepository _tickerDataRepository;

        public TickerService(IMapper mapper,
            ILogger<TickerService> logger,
            ITickerRepository tickerDataRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _tickerDataRepository = tickerDataRepository;
        }

        public async Task<TickerDto> GetTickerBySymbolAsync(string symbol)
        {
            try
            {
                var ticker = _mapper.Map<TickerDto?>(await _tickerDataRepository.GetTickerBySymbolAsync(symbol));
                if (ticker == null)
                {
                    var newTicker = new Ticker
                    {
                        Symbol = symbol.ToUpper()
                    };
                    newTicker = (await _tickerDataRepository.AddAsync(newTicker)).Entity;
                    await _tickerDataRepository.SaveChangesAsync();

                    ticker = _mapper.Map<TickerDto>(newTicker);
                }

                return ticker;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
