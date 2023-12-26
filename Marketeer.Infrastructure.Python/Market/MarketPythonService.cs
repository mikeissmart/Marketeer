﻿using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Domain.InfrastructureDto.Python.Market;
using Marketeer.Core.Domain.InfrastructureDtos.Python.Market;
using Marketeer.Persistance.Database.Repositories.Logging;

namespace Marketeer.Infrastructure.Python.Market
{
    public interface IMarketPythonService : IPythonService
    {
        Task DownloadTickerJsonInfo(List<string> tickers);
        Task<List<HistoryDataDto>> GetHistoryDataAsync(TickerDto ticker,
            HistoryDataIntervalEnum interval, DateTime? startDate, DateTime? endDate);
        Task<List<MarketScheduleDto>> GetYearlyMarketSchedule(int year);
    }

    public class MarketPythonService : BasePythonService, IMarketPythonService
    {
        private readonly IMapper _mapper;
        private readonly MarketPythonConfig _config;

        public MarketPythonService(IMapper mapper,
            RootPythonConfig rootPythonConfig,
            MarketPythonConfig config,
            IPythonLogRepository pythonLogRepository) : base(rootPythonConfig, config, pythonLogRepository)
        {
            _mapper = mapper;
            _config = config;
        }

        public async Task DownloadTickerJsonInfo(List<string> tickers)
        {
            var args = new PythonDownloadTickerJsonInfoArgs
            {
                Tickers = tickers
            };
            await RunPythonScriptArgsAsync(
                _config.DownloadTickerJsonInfo, args);
        }

        public async Task<List<HistoryDataDto>> GetHistoryDataAsync(TickerDto ticker,
            HistoryDataIntervalEnum interval, DateTime? startDate, DateTime? endDate)
        {
            var args = new PythonHistoryDataArgs
            {
                Ticker = ticker.Symbol,
                Interval = interval.ToIntervalString(),
                StartDate = startDate?.ToString("yyyy-MM-dd"),
                EndDate = endDate?.ToString("yyyy-MM-dd")
            };
            var pyHistDatas = await RunPythonScriptAsync<List<PythonHistoryDataDto>, PythonHistoryDataArgs>(
                _config.HistoryData, args);
            var histDatas = _mapper.Map<List<HistoryDataDto>>(pyHistDatas.Where(x =>
                x.Open.HasValue &&
                x.Close.HasValue &&
                x.High.HasValue &&
                x.Low.HasValue &
                x.Volume.HasValue));
            foreach (var hist in histDatas)
            {
                hist.TickerId = ticker.Id;
                hist.Interval = interval;
                hist.Date = hist.Date.Date;
            }

            return histDatas;
        }

        public async Task<List<MarketScheduleDto>> GetYearlyMarketSchedule(int year)
        {
            var args = new PythonMarketScheduleArgs
            {
                Year = year
            };

            var schedules = await RunPythonScriptAsync<List<PythonMarketScheduleDto>, PythonMarketScheduleArgs>(
                _config.GetYearlyMarketSchedule, args);

            var marketSchedules = new List<MarketScheduleDto>();
            foreach (var dto in schedules)
            {
                var item = new MarketScheduleDto();

                item.Day = dto.Day.Date;
                item.MarketOpen = dto.MarketOpen.ToUniversalTime();
                item.MarketClose = dto.MarketClose.ToUniversalTime();

                marketSchedules.Add(item);
            }

            return marketSchedules;
        }
    }
}
