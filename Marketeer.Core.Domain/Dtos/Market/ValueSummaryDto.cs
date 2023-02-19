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
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Avg { get; set; }

        public ValueSummaryDto() { }

        public ValueSummaryDto(string title, IEnumerable<decimal> values)
        {
            Title = title;
            Min = values.Min();
            Max = values.Max();
            Avg = values.Average();
        }
    }
}
