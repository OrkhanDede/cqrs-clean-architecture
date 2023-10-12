using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.Constants;

namespace Core.Extensions
{
    public enum DateRangeTypeEnum
    {
        Day,
        Month,
        Year
    }


    public class HMSFormatter : ICustomFormatter, IFormatProvider
    {
        // list of Formats, with a P customformat for pluralization
        static Dictionary<string, string> timeformats = new Dictionary<string, string>
        {
            {"S", "{0:P:Seconds:Second}"},
            {"M", "{0:P:Minutes:Minute}"},
            {"H", "{0:P:Hours:Hour}"},
            {"D", "{0:P:Days:Day}"}
        };

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return String.Format(new PluralFormatter(), timeformats[format], arg);
        }

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }

    // formats a numeric value based on a format P:Plural:Singular
    public class PluralFormatter : ICustomFormatter, IFormatProvider
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg != null)
            {
                var parts = format.Split(':'); // ["P", "Plural", "Singular"]

                if (parts[0] == "P") // correct format?
                {
                    // which index postion to use
                    int partIndex = (arg.ToString() == "1") ? 2 : 1;
                    // pick string (safe guard for array bounds) and format
                    return String.Format("{0} {1}", arg, (parts.Length > partIndex ? parts[partIndex] : ""));
                }
            }

            return String.Format(format, arg);
        }

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }


    public static class DateTimeExtensions
    {

        public static int GetIso8601WeekOfYear(this DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfIso8601Week(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static List<DateTime> GetDateRangeOfIso8601Week(int year, int weekOfYear)
        {
            var startDay = FirstDateOfIso8601Week(year, weekOfYear);
            var iterationDay = startDay;
            var dateRange = new List<DateTime>();
            do
            {
                dateRange.Add(iterationDay);
                iterationDay = iterationDay.AddDays(1);
            } while (iterationDay.DayOfWeek != DayOfWeek.Monday);

            return dateRange;
        }


        public static List<DateTime> Range(this DateTime startDate, DateTime endDate, DateRangeTypeEnum type)
        {
            var list = new List<DateTime>();
            if (type == DateRangeTypeEnum.Day)
            {
                for (DateTime counter = startDate; counter.Date <= endDate.Date; counter = counter.AddDays(1))
                {
                    list.Add(counter);
                }
            }

            else if (type == DateRangeTypeEnum.Month)

            {
                for (DateTime counter = startDate; counter.Date <= endDate.Date; counter = counter.AddMonths(1))
                {
                    list.Add(counter);
                }
            }
            else if (type == DateRangeTypeEnum.Year)
            {
                for (DateTime counter = startDate; counter.Date <= endDate.Date; counter = counter.AddYears(1))
                {
                    list.Add(counter);
                }
            }

            return list;
        }

        public static long ToUnixTimestamp(this DateTime value)
        {
            return (long)(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime FromUnixTimeStampToDateTime(this string unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp)).UtcDateTime;
        }

        public static String ToTimeStamp(this DateTime dt)
        {
            return dt.ToString(DateTimeConstants.DefaultTimeStampFormat);
        }

        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds = default,
            int milliseconds = default)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds, milliseconds,
                dateTime.Kind);
        }

        public static string GetReadableTimespan(this TimeSpan ts)
        {
            // formats and its cutoffs based on totalseconds
            var cutoff = new SortedList<long, string>
            {
                {59, "{3:S}"},
                {60, "{2:M}"},
                {60 * 60 - 1, "{2:M}, {3:S}"},
                {60 * 60, "{1:H}"},
                {24 * 60 * 60 - 1, "{1:H}, {2:M}"},
                {24 * 60 * 60, "{0:D}"},
                {Int64.MaxValue, "{0:D}, {1:H}"}
            };

            // find nearest best match
            var find = cutoff.Keys.ToList()
                .BinarySearch((long)ts.TotalSeconds);
            // negative values indicate a nearest match
            var near = find < 0 ? Math.Abs(find) - 1 : find;
            // use custom formatter to get the string
            return String.Format(
                new HMSFormatter(),
                cutoff[cutoff.Keys[near]],
                ts.Days,
                ts.Hours,
                ts.Minutes,
                ts.Seconds);
        }

        // formatter for forms of
        // seconds/hours/day
    }
}