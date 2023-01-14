using Marketeer.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class TickerSettingHistoryDisable : Entity
    {
        public int TickerSettingId { get; set; }
        public HistoryDataIntervalEnum Interval { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerSettingId))]
        public TickerSetting TickerSettings { get; set; }

        #endregion
    }
}
