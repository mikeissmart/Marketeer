namespace Marketeer.Common.Configs
{
    public class YFinancePythonConfig : PythonConfig, IConfig
    {
        public string HistoryDataFile { get; set; }
    }
}
