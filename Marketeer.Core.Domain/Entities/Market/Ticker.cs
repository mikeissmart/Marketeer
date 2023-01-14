using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class Ticker : Entity
    {
        [Required]
        [StringLength(5)]
        public string Symbol { get; set; }

        #region Nav

        public TickerInfo TickerInfo { get; set; }
        public TickerSetting TickerSetting { get; set; }
        public List<HistoryData> HistoryDatas { get; set; }

        #endregion
    }
}
