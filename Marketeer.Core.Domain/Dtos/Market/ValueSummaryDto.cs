using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class ValueSummaryDto : IRefactorType
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public decimal? Value { get; set; }
    }
}
