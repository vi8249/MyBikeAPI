using System;

namespace YouBikeAPI.Utilities
{
    public static class DateTimeExtension
    {
        public static bool IsBetween(this DateTime date, int start, int end)
        {
            if (date.Month <= DateTime.UtcNow.ToLocalTime().AddMonths(end).Month
                && date.Month > DateTime.UtcNow.ToLocalTime().AddMonths(start).Month)
                return true;
            return false;
        }
    }
}