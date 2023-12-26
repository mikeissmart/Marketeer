using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Dtos.AI;
using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Domain.InfrastructureDtos.Python.AI;
using Marketeer.Infrastructure.Python.AI;
using Marketeer.Persistance.Database.Repositories.AI;
using Marketeer.Persistance.Database.Repositories.News;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Service.AI
{
    public interface ISentimentService : ICoreService
    {
        Task QueuedToPendingSentimentsAsync();
        Task<List<SentimentResultDto>> GetLatestQueuedSentimentsAsync(int limit);
        Task<List<SentimentResultDto>> UpdateSentimentStatusAsync(List<SentimentResultDto> sentiments, SentimentStatusEnum status);
        Task<string> CalculateSentimentAsync(int huggingFaceModelId, List<SentimentResultDto> sentiments);
        Task<StringDataDto> EnqueueSentimentAsync(QueueSentimentDto queueDto);
    }

    public class SentimentService : BaseCoreService, ISentimentService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SentimentService> _logger;
        private readonly ISentimentResultRepository _sentimentRepository;
        private readonly IHuggingFaceModelRepository _huggingFaceModelRepository;
        private readonly ISentimentPythonService _sentimentPythonService;
        private readonly INewsArticleRepository _newsArticleRepository;

        public SentimentService(IMapper mapper,
            ILogger<SentimentService> logger,
            ISentimentResultRepository sentimentRepository,
            IHuggingFaceModelRepository huggingFaceModelRepository,
            ISentimentPythonService sentimentPythonService,
            INewsArticleRepository newsArticleRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _sentimentRepository = sentimentRepository;
            _huggingFaceModelRepository = huggingFaceModelRepository;
            _sentimentPythonService = sentimentPythonService;
            _newsArticleRepository = newsArticleRepository;
        }

        public async Task QueuedToPendingSentimentsAsync()
        {
            try
            {
                var sentiments = await _sentimentRepository.GetQueuedSentimentsAsync();
                foreach (var sent in sentiments)
                    sent.Status = SentimentStatusEnum.Pending;

                if (sentiments.Count > 0)
                {
                    _sentimentRepository.UpdateRange(sentiments);
                    await _sentimentRepository.SaveChangesAsync();
                    _sentimentRepository.ClearTracking();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<SentimentResultDto>> GetLatestQueuedSentimentsAsync(int limit)
        {
            try
            {
                return _mapper.Map<List<SentimentResultDto>>(await _sentimentRepository.GetLatestQueuedSentimentsAsync(limit, false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<SentimentResultDto>> UpdateSentimentStatusAsync(List<SentimentResultDto> sentiments, SentimentStatusEnum status)
        {
            try
            {
                var entities = _mapper.Map<List<SentimentResult>>(sentiments);

                foreach (var ent in entities)
                    ent.Status = status;

                _sentimentRepository.UpdateRange(entities);
                await _sentimentRepository.SaveChangesAsync();
                _sentimentRepository.ClearTracking();

                return _mapper.Map<List<SentimentResultDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<string> CalculateSentimentAsync(int huggingFaceModelId, List<SentimentResultDto> sentiments)
        {
            try
            {
                var huggingFace = await _huggingFaceModelRepository.GetHuggingFaceModelByIdAsync(huggingFaceModelId);
                if (huggingFace == null)
                    throw new ArgumentNullException($"Unable to find HuggingFaceModel by Id: {huggingFaceModelId}");

                var dic = new Dictionary<Guid, SentimentResultDto>();
                var queue = new List<SentimentQueueDto>();
                foreach (var sent in sentiments)
                {
                    if (huggingFace.Id != sent.HuggingFaceModelId)
                    {
                        var huggingFace2 = await _huggingFaceModelRepository.GetHuggingFaceModelByIdAsync(huggingFaceModelId);
                        throw new Exception($"Cant use different hugging face models: {huggingFace.Name} - {huggingFace2?.Name}");
                    }

                    var dto = new SentimentQueueDto
                    {
                        ItemGuid = Guid.NewGuid()
                    };

                    switch (sent.SentimentResultType)
                    {
                        case SentimentResultTypeEnum.NewsArticle:
                            dto.Text = sent.NewsArticle.Text;
                            break;
                        default:
                            throw new Exception($"Unknown SentimentResultType: {sent.SentimentResultType}");
                    }

                    queue.Add(dto);
                    dic.Add(dto.ItemGuid, sent);
                }

                var outQueue = await _sentimentPythonService.GetSentimentAsync(huggingFace.Name, queue);

                foreach (var oq in outQueue)
                {
                    var sent = dic[oq.ItemGuid];

                    sent.Negative = oq.Negative;
                    sent.Neutral = oq.Neutral;
                    sent.Positive = oq.Positive;

                    sent.Status = SentimentStatusEnum.Completed;
                }

                _sentimentRepository.UpdateRange(_mapper.Map<List<SentimentResult>>(sentiments));
                await _sentimentRepository.SaveChangesAsync();
                _sentimentRepository.ClearTracking();

                return $"HuggingFaceModel: {huggingFace.Name}, Sentiment Cout: {sentiments.Count}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<StringDataDto> EnqueueSentimentAsync(QueueSentimentDto queueDto)
        {
            try
            {
                var huggingFace = await _huggingFaceModelRepository.GetHuggingFaceModelByIdAsync(queueDto.HuggingFaceModelId);
                if (huggingFace == null)
                    throw new ArgumentNullException($"Unable to find HuggingFaceModel by Id: {queueDto.HuggingFaceModelId}");

                var enqueueCount = 0;
                var skipCount = 0;
                switch (queueDto.SentimentResultType)
                {
                    case SentimentResultTypeEnum.NewsArticle:
                        var news = await _newsArticleRepository.GetNewsArticlesByIdsAsync(queueDto.ItemIds);
                        var enqueue = news
                            .Where(x => !x.SentimentResults.Any(x => x.HuggingFaceModelId == queueDto.HuggingFaceModelId))
                            .ToList();

                        foreach (var n in enqueue)
                        {
                            n.SentimentResults.Add(new SentimentResult
                            {
                                HuggingFaceModelId = huggingFace.Id,
                                NewsArticleId = n.Id,
                                Status = SentimentStatusEnum.Pending
                            });
                        }
                        enqueueCount = enqueue.Count;
                        skipCount = news.Count - enqueue.Count;

                        _newsArticleRepository.UpdateRange(enqueue);
                        break;
                    default:
                        throw new Exception($"Unknown SentimentResultType: {queueDto.SentimentResultType}");
                }

                await _huggingFaceModelRepository.SaveChangesAsync();

                return new StringDataDto { Data = $"Enqueued: {enqueueCount}, Skipped: {skipCount}, Totoal: {enqueueCount + skipCount}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
