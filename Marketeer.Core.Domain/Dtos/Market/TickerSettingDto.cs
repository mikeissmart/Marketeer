using Marketeer.Core.Domain.Entities.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerSettingDto : EntityDto, IRefactorType,
        IMapFrom<TickerSetting>,
        IMapTo<TickerSetting>
    {
        public int TickerId { get; set; }
        public bool IsDelisted { get; set; }
        public bool UpdateDailyHistory { get; set; }
    }
}
