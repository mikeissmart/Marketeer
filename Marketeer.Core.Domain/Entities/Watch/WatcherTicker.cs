using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.Core.Domain.Entities.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.Watch
{
    public class WatchTicker : Entity
    {
        public int AppUserId { get; set; }
        public int TickerId { get; set; }
        public bool UpdateHistoryData { get; set; }
        public bool UpdateNewsArticles { get; set; }

        #region Nav

        public AppUser AppUser { get; set; }
        public Ticker Ticker { get; set; }

        #endregion
    }
}
