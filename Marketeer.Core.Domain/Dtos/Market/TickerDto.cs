using Marketeer.Core.Domain.Entities.Market;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerDto : EntityDto, IRefactorType,
        IMapFrom<Ticker>,
        IMapTo<Ticker>
    {
        public string Symbol { get; set; }

        public TickerInfoDto TickerInfo { get; set; }
        public TickerSettingDto TickerSetting { get; set; }
    }
}
