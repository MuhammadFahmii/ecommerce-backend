// ------------------------------------------------------------------------------------
// DateExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace netca.Application.Common.Extensions;

/// <summary>
/// DateExtensions
/// </summary>
public static class DateExtensions
{
    /// <summary>
    /// DateCompare
    /// </summary>
    /// <param name="date1"></param>
    /// <param name="date2"></param>
    /// <returns></returns>
    public static int DateCompare(DateTime date1, DateTime date2)
    {
        return DateTime.Compare(date1, date2);
    }

    /// <summary>
    /// IsDateFormat
    /// </summary>
    /// <param name="date"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static bool IsDateFormat(string date, string format)
    {
        return DateTime.TryParseExact(date, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out _);
    }

    /// <summary>
    /// FormatDate
    /// </summary>
    /// <param name="date"></param>
    /// <param name="formatDate"></param>
    /// <param name="time"></param>
    /// <param name="addHour"></param>
    /// <returns></returns>
    public static DateTime? FormatDate(string date, string formatDate, string time, int addHour)
    {
        if (IsDateFormat(date, formatDate))
        {
            var result = DateTime.ParseExact(date, formatDate, CultureInfo.CurrentCulture);
            if (time.Length == 5)
                time = "0" + time;
            var timeSpanFormat = new TimeSpan(0, int.Parse(time[..2]), int.Parse(time.Substring(2, 2)),
                int.Parse(time.Substring(4, 2)));

            result += timeSpanFormat;
            return result.AddHours(addHour);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// GetUnixTimestamp
    /// </summary>
    /// <returns></returns>
    public static long GetUnixTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}