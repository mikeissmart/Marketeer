using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.InfrastructureDtos.Python.Market
{
    public class PythonMarketScheduleDto
    {
        public DateTime Date { get; set; }
        public DateTime MarketOpenDateTime { get; set; }
        public DateTime MarketCloseDateTime { get; set; }
    }
}
