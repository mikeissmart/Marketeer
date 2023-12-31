using Marketeer.Core.Domain.Dtos.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Watch
{
    public class WatchTickerDetailsChangeDto : IRefactorType
    {
        public bool UpdateHistoryData { get; set; }
        public bool UpdateNewsArticles { get; set; }
        public PaginateFilterDto<TickerFilterDto> Filter { get; set; }
    }
}
