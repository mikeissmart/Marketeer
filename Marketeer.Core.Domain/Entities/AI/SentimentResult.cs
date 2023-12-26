using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Entities.News;
using Marketeer.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.AI
{
    public class SentimentResult : EntityAuditCreateUpdate
    {
        public int HuggingFaceModelId { get; set; }
        public int? NewsArticleId { get; set; }
        public float Negative { get; set; }
        public float Neutral { get; set; }
        public float Positive { get; set; }
        public SentimentStatusEnum Status { get; set; }

        #region Nav

        public HuggingFaceModel HuggingFaceModel { get; set; }
        public NewsArticle NewsArticle { get; set; }

        #endregion

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
