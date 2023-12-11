using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerHistorySummaryDto : IRefactorType
    {
        public List<ValueSummaryDto> ValueSummaries { get; set; }
    }
}
