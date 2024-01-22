using System;

namespace CaseMix.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWithinDateRage(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck.Date >= startDate.Date && dateToCheck.Date <= endDate.Date;
        }
    }
}
