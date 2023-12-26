using Marketeer.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.AI
{
    public class QueueSentimentDto : IRefactorType
    {
        public int HuggingFaceModelId { get; set; }
        public SentimentResultTypeEnum SentimentResultType { get; set; }
        public List<int> ItemIds { get; set; }
    }
}
