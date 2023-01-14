namespace Marketeer.Common.Configs
{
    public class CronConfig : IConfig
    {
        public string ClearTickerSettingsTempHistoryDisable { get; set; }
        public string RefreshTickersCronJob { get; set; }
    }
}
