using Abp.Domain.Services;

namespace CaseMix
{
    public class CaseMixDomainServiceBase : DomainService
    {
        public CaseMixDomainServiceBase()
        {
            LocalizationSourceName = CaseMixConsts.LocalizationSourceName;
        }
    }
}
