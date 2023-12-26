using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Service.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Cron.AI
{
    public class CalculateSentimentCronQueue : BaseCronQueueService
    {
        private readonly CalculateSentimentCronQueueConfig _config;

        public CalculateSentimentCronQueue(IHostApplicationLifetime applicationLifetime, IServiceProvider services, CalculateSentimentCronQueueConfig config) :
            base(applicationLifetime, services, config)
        {
            _config = config;
        }

        protected override async Task<string?> DoQueueAsync(IServiceScope scope, CancellationToken cancellationToken)
        {

            var sentimentService = scope.ServiceProvider.GetRequiredService<ISentimentService>();

            await sentimentService.QueuedToPendingSentimentsAsync();

            var queue = await sentimentService.GetLatestQueuedSentimentsAsync(_config.BatchLimit);

            try
            {
                if (queue.Count > 0)
                {
                    queue = await sentimentService.UpdateSentimentStatusAsync(queue, SentimentStatusEnum.Queued);

                    var grp = queue
                        .GroupBy(x => x.HuggingFaceModelId, x => x)
                        .ToDictionary(x => x.Key, x => x.ToList());

                    var results = new List<string>();
                    foreach (var pair in grp)
                        results.Add(await sentimentService.CalculateSentimentAsync(pair.Key, pair.Value));

                    return string.Join(", ", results);
                }
            }
            catch (Exception ex)
            {
                await sentimentService.UpdateSentimentStatusAsync(queue, SentimentStatusEnum.Failed);
                throw;
            }

            return null;
        }
    }
}
