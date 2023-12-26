using Marketeer.Core.Domain.Entities.News;
using Marketeer.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.AI
{
    public class HuggingFaceModel : Entity
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        #region Nav

        public List<SentimentResult> SentimentResults { get; set; }

        #endregion
    }
}
