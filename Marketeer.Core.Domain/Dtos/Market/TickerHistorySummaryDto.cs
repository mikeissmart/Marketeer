using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerHistorySummaryDto : IRefactorType
    {
        public DateTime? EarliestDate { get; set; }
        public DateTime? LatestDate { get; set; }
        public List<ValueSummaryDto> ValueSummaries { get; set; }
    }
}
