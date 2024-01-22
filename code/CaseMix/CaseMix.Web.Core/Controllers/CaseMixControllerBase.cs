using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace CaseMix.Controllers
{
    public abstract class CaseMixControllerBase: AbpController
    {
        protected CaseMixControllerBase()
        {
            LocalizationSourceName = CaseMixConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
