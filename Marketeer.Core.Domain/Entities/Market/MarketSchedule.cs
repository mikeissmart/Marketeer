using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class MarketSchedule : Entity
    {
        public DateTime Day { get; set; }
        public DateTime MarketOpen { get; set; }
        public DateTime MarketClose { get; set; }
    }
}
