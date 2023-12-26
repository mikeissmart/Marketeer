using Marketeer.Core.Domain.Dtos.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.InfrastructureDtos.Python.AI
{
    public class CalculateSentimentArgs
    {
        public string HuggingFaceModel { get; set; }
        public List<SentimentQueueDto> Queue { get; set; }
    }
}
