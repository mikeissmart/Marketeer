namespace Marketeer.Common.Configs
{
    public class TickerConfig : IConfig
    {
        public float TickerInfoRefreshPercent { get; set; }
        public int HistoryDataRetryDays { get; set; }
    }
}
