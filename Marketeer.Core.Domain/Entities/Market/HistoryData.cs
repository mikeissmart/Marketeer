using Marketeer.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class HistoryData : Entity
    {
        public int TickerId { get; set; }
        public HistoryDataIntervalEnum Interval { get; set; }
        [Column(TypeName = "decimal(19,10)")]
        public decimal Open { get; set; }
        [Column(TypeName = "decimal(19,10)")]
        public decimal Close { get; set; }
        [Column(TypeName = "decimal(19,10)")]
        public decimal High { get; set; }
        [Column(TypeName = "decimal(19,10)")]
        public decimal Low { get; set; }
        public long Volume { get; set; }
        public DateTime DateTime { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerId))]
        public Ticker Ticker { get; set; }

        #endregion
    }
}
