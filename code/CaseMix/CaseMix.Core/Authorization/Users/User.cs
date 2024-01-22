using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using CaseMix.Entities;
using CaseMix.Entities.Enums;

namespace CaseMix.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public virtual SurgeonExperienceType? Experience { get; set; }
        public Guid? ManufactureId { get; set; }

        [ForeignKey("ManufactureId")]
        public Manufacture Manufacture { get; set; }

        public ICollection<UserRealmMapping> UserRealmMappings { get; set; }
    }
}
