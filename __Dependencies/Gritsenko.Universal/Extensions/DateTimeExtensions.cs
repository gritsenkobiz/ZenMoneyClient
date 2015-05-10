using System;

namespace Gritsenko.Universal.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToTimeStamp(this DateTime dateTime)
        {
            return (long)Math.Round(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            // Java timestamp is millisecods past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(Math.Round(javaTimeStamp / 1000)).ToLocalTime();
            return dtDateTime;
        }

        public static string FormatDate(this DateTime date)
        {
            var now = DateTime.Now.Date;

            if (now == date)
            {
                return "Сегодня";
            }

            if (now.AddDays(-1) == date)
            {
                return "Вчера";
            }

            return date.ToString("d MMMM, dddd");
        }

    }
}
