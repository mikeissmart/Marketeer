using Marketeer.Core.Domain.Dtos.Market;
using Marketeer.Core.Domain.Entities.Watch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Watch
{
    public class WatchTickerDto : EntityDto, IRefactorType,
        IMapFrom<WatchTicker>,
        IMapTo<WatchTicker>
    {
        public bool UpdateHistoryData { get; set; }
        public bool UpdateNewsArticles { get; set; }
    }
}
