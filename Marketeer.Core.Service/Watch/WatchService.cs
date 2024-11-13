using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.Watch;
using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Entities.Watch;
using Marketeer.Persistance.Database.Repositories.Auth;
using Marketeer.Persistance.Database.Repositories.Watch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Service.Watch
{
    public interface IWatchTickerService : ICoreService
    {
        Task<WatchTickerDto?> GetWatchTickerUpdateDailyAsync(int tickerId, int userId);
        Task<WatchTickerResultDto> UpdateWatchTickerUpdateDailyAsync(WatchTickerChangeDto changeDto, int userId);
        Task<WatchUserStatusDto> GetWatcherUserStatusAsync(int userId);
    }

    public class WatchTickerService : BaseCoreService, IWatchTickerService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<WatchTickerService> _logger;
        private readonly WatchTickerConfig _watchTickerConfig;
        private readonly IWatchTickerRepository _watchTickerRepository;
        private readonly IAppUserRepository _appUserRepository;

        public WatchTickerService(IMapper mapper, ILogger<WatchTickerService> logger, WatchTickerConfig WatchTickerConfig, IWatchTickerRepository WatchTickerRepository, IAppUserRepository appUserRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _watchTickerConfig = WatchTickerConfig;
            _watchTickerRepository = WatchTickerRepository;
            _appUserRepository = appUserRepository;
        }

        public async Task<WatchTickerDto?> GetWatchTickerUpdateDailyAsync(int tickerId, int userId)
        {
            try
            {
                var watchTicker = await _watchTickerRepository.GetWatchTickerUpdateTickerDailiesByTickerIdAndUserAsync(tickerId, userId);

                return _mapper.Map<WatchTickerDto>(watchTicker ?? new WatchTicker());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<WatchTickerResultDto> UpdateWatchTickerUpdateDailyAsync(WatchTickerChangeDto changeDto, int userId)
        {
            try
            {
                var result = new WatchTickerResultDto
                {
                    MaxCount = _watchTickerConfig.MaxWatchTickerPerUser
                };

                var user = await _appUserRepository.GetUserByIdAsync(userId, withWatchTicker: true);
                if (user == null)
                    throw new ArgumentNullException();
                result.CurrentCount = user.WatchTickers.Count();

                var currentWatchTickers = user.WatchTickers
                    .Where(x => changeDto.TickerIds.Contains(x.TickerId))
                    .ToList();
                foreach (var watchTicker in currentWatchTickers)
                {
                    if (changeDto.UpdateHistoryData || changeDto.UpdateNewsArticles)
                    {
                        result.UpdatedCount++;

                        watchTicker.UpdateHistoryData = changeDto.UpdateHistoryData;
                        watchTicker.UpdateNewsArticles = changeDto.UpdateNewsArticles;

                        _watchTickerRepository.Update(watchTicker);
                    }
                    else
                    {
                        result.CurrentCount--;
                        result.RemovedCount++;
                        _watchTickerRepository.Remove(watchTicker);
                    }
                }

                if (changeDto.UpdateHistoryData || changeDto.UpdateNewsArticles)
                {
                    var newTickerIds = changeDto.TickerIds
                    .Where(x => !currentWatchTickers.Any(y => y.TickerId == x))
                    .ToList();
                    var newWatchTickers = new List<WatchTicker>();
                    foreach (var tickerId in newTickerIds)
                    {
                        if (_watchTickerConfig.MaxWatchTickerPerUser != -1 &&
                            result.CurrentCount + result.AddedCount + 1 > _watchTickerConfig.MaxWatchTickerPerUser)
                            break;

                        newWatchTickers.Add(new WatchTicker
                        {
                            AppUserId = userId,
                            TickerId = tickerId,
                            UpdateHistoryData = changeDto.UpdateHistoryData,
                            UpdateNewsArticles = changeDto.UpdateNewsArticles
                        });
                        result.AddedCount++;
                    }
                    await _watchTickerRepository.AddRangeAsync(newWatchTickers);
                }

                await _watchTickerRepository.SaveChangesAsync();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<WatchUserStatusDto> GetWatcherUserStatusAsync(int userId)
        {
            try
            {
                var status = new WatchUserStatusDto();

                status.WatchTickerCount = await _watchTickerRepository.GetUserWatchTickerCountAsync(userId);
                status.WatchTickerCountMax = _watchTickerConfig.MaxWatchTickerPerUser;

                return status;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
