using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class Ticker : Entity
    {
        [Required]
        [StringLength(5)]
        public string Symbol { get; set; }
        public DateTime? LastHistoryUpdate { get; set; }

        #region Info

        //shortName
        [StringLength(100)]
        public string Name { get; set; } = "";
        //quoteType
        [StringLength(25)]
        public string QuoteType { get; set; } = "";
        //exchange
        [StringLength(10)]
        public string Exchange { get; set; } = "";
        //marketCap
        public long? MarketCap { get; set; }
        //sector
        [StringLength(50)]
        public string? Sector { get; set; }
        //industry
        [StringLength(50)]
        public string? Industry { get; set; }
        //volume
        public long? Volume { get; set; }
        //payoutRatio
        public float? PayoutRatio { get; set; }
        //dividendRate
        public float? DividendRate { get; set; }
        public DateTime? LastInfoUpdate { get; set; }

        #endregion

        #region Nav

        public List<TickerDelistReason> DelistReasons { get; set; }
        public List<HistoryData> HistoryDatas { get; set; }

        #endregion

        #region Not Mapped

        [NotMapped]
        public bool IsDelisted { get => DelistReasons.Any(); }

        #endregion
    }
}
