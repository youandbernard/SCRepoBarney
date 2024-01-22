using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneConverter;

namespace CaseMix.DomainServices.Timezone
{
    public class TimezoneDomainService : CaseMixDomainServiceBase, ITimezoneDomainService
    {
        public TimezoneDomainService()
        {

        }

        public DateTime ConvertToUtc(DateTime dateStart, string timezone)
        {
            var timeZone = TZConvert.GetTimeZoneInfo(timezone);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone.Id);
            var dateStartUtc = TimeZoneInfo.ConvertTimeToUtc(dateStart, timeZoneInfo);

            return dateStartUtc;
        }

        public DateTime ConvertToLocal(DateTime dateStart, string timezone)
        {
            var timeZone = TZConvert.GetTimeZoneInfo(timezone);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone.Id);
            var dateStartUtc = TimeZoneInfo.ConvertTimeFromUtc(dateStart, timeZoneInfo);

            return dateStartUtc;
        }
    }
}
