using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using Marketeer.Core.Domain.InfrastructureDtos.Python.Market;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class HistoryDataDto : EntityDto, IRefactorType,
        IMapFrom<HistoryData>,
        IMapFrom<PythonHistoryDataDto>,
        IMapTo<HistoryData>
    {
        public int TickerId { get; set; }
        public HistoryDataIntervalEnum Interval { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
        public DateTime DateTime { get; set; }
    }
}
