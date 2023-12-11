using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Entities.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.AI
{
    public class SentimentResult : EntityAuditCreate
    {
        public int HuggingFaceModelId { get; set; }
        public int? NewsArticleId { get; set; }
        public float Negative { get; set; }
        public float Neutral { get; set; }
        public float Positive { get; set; }

        #region Nav

        public HuggingFaceModel HuggingFaceModel { get; set; }
        public NewsArticle NewsArticle { get; set; }

        #endregion
    }
}
