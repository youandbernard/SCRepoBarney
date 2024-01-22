using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.DomainServices.Timezone
{
    public interface ITimezoneDomainService : IDomainService
    {
        DateTime ConvertToUtc(DateTime dateStart, string timezone);
        DateTime ConvertToLocal(DateTime dateStart, string timezone);
    }
}
