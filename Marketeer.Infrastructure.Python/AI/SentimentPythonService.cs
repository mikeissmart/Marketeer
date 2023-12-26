using AutoMapper;
using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Dtos.AI;
using Marketeer.Core.Domain.InfrastructureDtos.Python.AI;
using Marketeer.Persistance.Database.Repositories.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Infrastructure.Python.AI
{
    public interface ISentimentPythonService : IPythonService
    {
        Task<List<SentimentQueueDto>> GetSentimentAsync(string huggingFaceModelName, List<SentimentQueueDto> queue);
    }

    public class SentimentPythonService : BasePythonService, ISentimentPythonService
    {
        private readonly IMapper _mapper;
        private readonly AIPythonConfig _config;

        public SentimentPythonService(IMapper mapper,
            RootPythonConfig rootPythonConfig,
            AIPythonConfig config,
            IPythonLogRepository pythonLogRepository) : base(rootPythonConfig, config, pythonLogRepository)
        {
            _mapper = mapper;
            _config = config;
        }

        public async Task<List<SentimentQueueDto>> GetSentimentAsync(string huggingFaceModelName, List<SentimentQueueDto> queue)
        {
            var args = new CalculateSentimentArgs
            {
                HuggingFaceModel = huggingFaceModelName,
                Queue = queue
            };

            var result = await RunPythonScriptAsync<CalculateSentimentDto, CalculateSentimentArgs>(_config.CalculateSentiment, args);

            return result.Queue;
        }
    }
}
