using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.InfrastructureDtos.Python.Market;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerInfoDto : EntityDto, IRefactorType,
        IMapFrom<TickerInfo>,
        IMapFrom<PythonTickerInfoDto>,
        IMapTo<TickerInfo>
    {
        public int TickerId { get; set; }
        public string Name { get; set; } = "";
        public string QuoteType { get; set; } = "";
        public string Exchange { get; set; } = "";
        public long? MarketCap { get; set; }
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public long? Volume { get; set; }
        public float? PayoutRatio { get; set; }
        public float? DividendRate { get; set; }
    }
}
