using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class TickerInfo : EntityAuditUpdate
    {
        public int TickerId { get; set; }
        public string Name { get; set; } = "";
        public string QuoteType { get; set; } = "";
        public string Sector { get; set; } = "";
        public string Industry { get; set; } = "";
        public long Volume { get; set; }
        public bool IsDelisted { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerId))]
        public Ticker Ticker { get; set; }

        #endregion
    }
}
