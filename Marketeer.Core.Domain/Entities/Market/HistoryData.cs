using Marketeer.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class HistoryData : Entity
    {
        public int TickerId { get; set; }
        public HistoryDataIntervalEnum Interval { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
        public DateTime Date { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerId))]
        public Ticker Ticker { get; set; }

        #endregion
    }
}
