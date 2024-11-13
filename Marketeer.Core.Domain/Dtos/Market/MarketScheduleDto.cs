using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.InfrastructureDtos.Python.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class MarketScheduleDto : EntityDto, IRefactorType,
        IMapFrom<PythonMarketScheduleDto>,
        IMapFrom<MarketSchedule>,
        IMapTo<MarketSchedule>
    {
        public DateTime Date { get; set; }
    }
}
