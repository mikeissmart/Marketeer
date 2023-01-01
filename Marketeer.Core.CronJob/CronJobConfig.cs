namespace Marketeer.Core.CronJob
{
    public interface ICronJobConfig
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    public interface ICronJobConfig<T> : ICronJobConfig
    { }

    public class CronJobConfig<T> : ICronJobConfig<T>
    {
        public string CronExpression { get; set; } = "";
        public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
    }
}
