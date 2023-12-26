namespace Marketeer.Common.Configs
{
    public class CronJobConfig : IConfig
    {
        public string RefreshTickers { get; set; }
        public string UpdateDailyHistoryData { get; set; }
        public string GetYearlyMarketSchedule { get; set; }
    }
}
