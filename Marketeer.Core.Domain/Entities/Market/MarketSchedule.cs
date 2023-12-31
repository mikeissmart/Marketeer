using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class MarketSchedule : Entity
    {
        public DateTime Date { get; set; }
        public DateTime MarketOpenDateTime { get; set; }
        public DateTime MarketCloseDateTime { get; set; }
    }
}
