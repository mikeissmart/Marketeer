namespace Marketeer.Common.Configs
{
    public class YFinancePythonConfig : PythonConfig, IConfig
    {
        public string HistoryDataFile { get; set; }
        public string DownloadTickerInfoJsonFile { get; set; }
    }
}
