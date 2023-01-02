using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class HistoryDataDto : EntityDto, IRefactorType, IMapFrom<HistoryData>, IMapTo<HistoryData>
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
