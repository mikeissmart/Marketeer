using Marketeer.Core.Domain.Entities.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class MarketScheduleDto : EntityDto, IRefactorType,
        IMapFrom<MarketSchedule>,
        IMapTo<MarketSchedule>
    {
        public DateTime Day { get; set; }
        public DateTime MarketOpen { get; set; }
        public DateTime MarketClose { get; set; }
    }
}
