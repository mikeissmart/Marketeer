namespace Marketeer.Common.Configs
{
    public class TickerConfig : IConfig
    {
        public float TickerInfoRefreshPercent { get; set; }
        public int HistoryDataRetryDays { get; set; }
        public int HistoryDataKeepNowYearMinusYear { get; set; }
        public DateTime MinHistoryDataYearlyDate { get => new DateTime(DateTime.Now.Year - HistoryDataKeepNowYearMinusYear, 1, 1); }
    }
}
