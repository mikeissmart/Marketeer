using Marketeer.Core.Domain.Dtos.News;
using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.AI
{
    public class SentimentResultDto : EntityAuditCreateUpdateDto, IRefactorType,
        IMapFrom<SentimentResult>,
        IMapTo<SentimentResult>
    {
        public int HuggingFaceModelId { get; set; }
        public int? NewsArticleId { get; set; }
        public float Negative { get; set; }
        public float Neutral { get; set; }
        public float Positive { get; set; }
        public SentimentStatusEnum Status { get; set; }

        public HuggingFaceModelDto HuggingFaceModel { get; set; }

        public NewsArticleDto NewsArticle { get; set; }

        public SentimentResultTypeEnum SentimentResultType
        {
            get
            {
                if (NewsArticleId != null)
                    return SentimentResultTypeEnum.NewsArticle;
                else
                    throw new Exception($"Unknown SentimentResultType, NewsArticleId ({NewsArticleId})");
            }
        }

        public SentimentEnum Sentiment
        {
            get
            {
                if (Negative > Neutral && Negative > Positive)
                    return SentimentEnum.Negative;
                else if (Neutral > Negative && Neutral > Positive)
                    return SentimentEnum.Neutral;
                else// if (Positive > Negative && Positive > Neutral)
                    return SentimentEnum.Positive;
            }
        }
    }
}
