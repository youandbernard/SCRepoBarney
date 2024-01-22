using Abp.Authorization;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;

namespace CaseMix.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
