﻿using Marketeer.Core.Domain.Dtos.Watch;
using Marketeer.Core.Domain.Entities.Market;
using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerDto : EntityDto, IRefactorType,
        IMapFrom<Ticker>,
        IMapTo<Ticker>
    {
        public string Symbol { get; set; }
        public DateTime? LastHistoryUpdateDateTime { get; set; }
        public DateTime? LastInfoUpdateDateTime { get; set; }
        public DateTime? LastNewsUpdateDateTime { get; set; }
        public string Name { get; set; } = "";
        public string QuoteType { get; set; } = "";
        public string Exchange { get; set; } = "";
        public long? MarketCap { get; set; }
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public long? Volume { get; set; }
        public float? PayoutRatio { get; set; }
        public float? DividendRate { get; set; }
        public List<TickerDelistReasonDto> DelistReasons { get; set; }
        public List<WatchTickerDto> WatchTickers { get; set; }
    }
}
