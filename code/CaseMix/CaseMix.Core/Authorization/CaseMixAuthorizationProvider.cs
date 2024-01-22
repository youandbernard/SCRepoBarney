using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace CaseMix.Authorization
{
    public class CaseMixAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Dashboard, L("Dashboard"));
            context.CreatePermission(PermissionNames.Pages_Surveys, L("Surveys"));
            context.CreatePermission(PermissionNames.Pages_Poaps, L("POAPs"));
            context.CreatePermission(PermissionNames.Pages_Devices, L("Devices"));
            context.CreatePermission(PermissionNames.Pages_Theaters, L("Theaters"));
            context.CreatePermission(PermissionNames.Pages_Settings, L("Settings"));
            context.CreatePermission(PermissionNames.Pages_Region_Management, L("RegionManagement"));
            context.CreatePermission(PermissionNames.Pages_Hospital_Management, L("HospitalManagement"));
            context.CreatePermission(PermissionNames.Pages_Device_Management, L("DeviceManagement"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CaseMixConsts.LocalizationSourceName);
        }
    }
}
