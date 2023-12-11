namespace Marketeer.Common.Configs
{
    public class MarketPythonConfig : PythonConfig, IConfig
    {
        public string DownloadTickerJsonInfo { get; set; }
        public string HistoryData { get; set; }
        public string LongWait { get; set; }
        public string GetYearlyMarketSchedule { get; set; }
    }
}
