using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class TickerInfo : EntityAuditUpdate
    {
        public int TickerId { get; set; }
        //shortName
        public string Name { get; set; } = "";
        //quoteType
        public string QuoteType { get; set; } = "";
        //exchange
        public string Exchange { get; set; } = "";
        //marketCap
        public long? MarketCap { get; set; }
        //sector
        public string? Sector { get; set; }
        //industry
        public string? Industry { get; set; }
        //volume
        public long? Volume { get; set; }
        //payoutRatio
        public float? PayoutRatio { get; set; }
        //dividendRate
        public float? DividendRate { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerId))]
        public Ticker Ticker { get; set; }

        #endregion
    }
}
