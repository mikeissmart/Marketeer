namespace Marketeer.Common.Configs
{
    public class MarketPythonConfig : PythonConfig, IConfig
    {
        public string DownloadTickerInfoJson { get; set; }
        public string HistoryData { get; set; }
        public string GetYearlyMarketSchedule { get; set; }
    }
}
