using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Common.Configs
{
    public class WatchTickerConfig : IConfig
    {
        public int MaxWatchTickerPerUser { get; set; }
    }
}
