using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.AI
{
    public class SentimentQueueDto
    {
        public Guid ItemGuid { get; set; }
        public string Text { get; set; }
        public float Negative { get; set; }
        public float Neutral { get; set; }
        public float Positive { get; set; }
    }
}
