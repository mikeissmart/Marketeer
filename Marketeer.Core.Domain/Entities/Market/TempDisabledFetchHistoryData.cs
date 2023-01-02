using Marketeer.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class TempDisabledFetchHistoryData : Entity
    {
        public int TickerId { get; set; }
        public HistoryDataIntervalEnum Interval { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerId))]
        public Ticker Ticker { get; set; }

        #endregion
    }
}
