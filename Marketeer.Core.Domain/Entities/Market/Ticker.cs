using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class Ticker : Entity
    {
        [StringLength(4)]
        public string Symbol { get; set; }
        public bool IsDelisted { get; set; }

        #region Nav

        public TempDisabledFetchHistoryData TempDisabledFetchHistoryData { get; set; }
        public List<HistoryData> HistoryDatas { get; set; }

        #endregion
    }
}
