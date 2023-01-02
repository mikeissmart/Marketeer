namespace Marketeer.Core.Domain.Enums
{
    public enum HistoryDataIntervalEnum
    {
        One_Minute,
        Two_Minutes,
        Five_Minutes,
        Fifteen_Minutes,
        Thirty_Minutes,
        Sixty_Minutes,
        Ninety_Minutes,
        One_Day,
        Five_Days,
        One_Week,
        One_Month,
        Three_Months
    }

    public static class HistoryDataIntervalToString
    {
        public static string ToIntervalString(this HistoryDataIntervalEnum interval)
        {
            switch (interval)
            {
                case HistoryDataIntervalEnum.One_Minute:
                    return "1m";
                case HistoryDataIntervalEnum.Two_Minutes:
                    return "2m";
                case HistoryDataIntervalEnum.Five_Minutes:
                    return "5m";
                case HistoryDataIntervalEnum.Fifteen_Minutes:
                    return "15m";
                case HistoryDataIntervalEnum.Thirty_Minutes:
                    return "30m";
                case HistoryDataIntervalEnum.Sixty_Minutes:
                    return "60m";
                case HistoryDataIntervalEnum.Ninety_Minutes:
                    return "90m";
                case HistoryDataIntervalEnum.One_Day:
                    return "1d";
                case HistoryDataIntervalEnum.Five_Days:
                    return "5d";
                case HistoryDataIntervalEnum.One_Week:
                    return "1wk";
                case HistoryDataIntervalEnum.One_Month:
                    return "1mo";
                case HistoryDataIntervalEnum.Three_Months:
                    return "3mo";
                default:
                    throw new NotImplementedException();
            }
        }

        public static DateTime AddInterval(this HistoryDataIntervalEnum interval, DateTime dateTime)
        {
            switch (interval)
            {
                case HistoryDataIntervalEnum.One_Minute:
                    return dateTime.AddMinutes(1);
                case HistoryDataIntervalEnum.Two_Minutes:
                    return dateTime.AddMinutes(2);
                case HistoryDataIntervalEnum.Five_Minutes:
                    return dateTime.AddMinutes(5);
                case HistoryDataIntervalEnum.Fifteen_Minutes:
                    return dateTime.AddMinutes(15);
                case HistoryDataIntervalEnum.Thirty_Minutes:
                    return dateTime.AddMinutes(30);
                case HistoryDataIntervalEnum.Sixty_Minutes:
                    return dateTime.AddMinutes(60);
                case HistoryDataIntervalEnum.Ninety_Minutes:
                    return dateTime.AddMinutes(90);
                case HistoryDataIntervalEnum.One_Day:
                    return dateTime.AddDays(1);
                case HistoryDataIntervalEnum.Five_Days:
                    return dateTime.AddDays(5);
                case HistoryDataIntervalEnum.One_Week:
                    return dateTime.AddDays(7);
                case HistoryDataIntervalEnum.One_Month:
                    return dateTime.AddMonths(1);
                case HistoryDataIntervalEnum.Three_Months:
                    return dateTime.AddMonths(3);
                default:
                    throw new NotImplementedException();
            }
        }

        public static DateTime MinusInterval(this HistoryDataIntervalEnum interval, DateTime dateTime)
        {
            switch (interval)
            {
                case HistoryDataIntervalEnum.One_Minute:
                    return dateTime.AddMinutes(-1);
                case HistoryDataIntervalEnum.Two_Minutes:
                    return dateTime.AddMinutes(-2);
                case HistoryDataIntervalEnum.Five_Minutes:
                    return dateTime.AddMinutes(-5);
                case HistoryDataIntervalEnum.Fifteen_Minutes:
                    return dateTime.AddMinutes(-15);
                case HistoryDataIntervalEnum.Thirty_Minutes:
                    return dateTime.AddMinutes(-30);
                case HistoryDataIntervalEnum.Sixty_Minutes:
                    return dateTime.AddMinutes(-60);
                case HistoryDataIntervalEnum.Ninety_Minutes:
                    return dateTime.AddMinutes(-90);
                case HistoryDataIntervalEnum.One_Day:
                    return dateTime.AddDays(-1);
                case HistoryDataIntervalEnum.Five_Days:
                    return dateTime.AddDays(-5);
                case HistoryDataIntervalEnum.One_Week:
                    return dateTime.AddDays(-7);
                case HistoryDataIntervalEnum.One_Month:
                    return dateTime.AddMonths(-1);
                case HistoryDataIntervalEnum.Three_Months:
                    return dateTime.AddMonths(-3);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
