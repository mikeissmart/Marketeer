using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Watch
{
    public class WatchTickerChangeDto : IRefactorType
    {
        public List<int> TickerIds { get; set; }
        public bool UpdateHistoryData { get; set; }
        public bool UpdateNewsArticles { get; set; }
    }
}
