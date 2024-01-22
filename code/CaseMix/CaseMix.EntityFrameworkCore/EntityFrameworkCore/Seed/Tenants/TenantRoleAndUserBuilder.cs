using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using CaseMix.Authorization;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using System.Collections.Generic;

namespace CaseMix.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly CaseMixDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(CaseMixDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void GrantPermissions(Role role, string[] permissionNames)
        {
            var grantedAdminPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == role.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new CaseMixAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedAdminPermissions.Contains(p.Name) &&
                            permissionNames.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = role.Id
                    })
                );
                _context.SaveChanges();
            }
        }

        private void CreateRolesAndUsers()
        {
            // Super Admin role

            var superAdminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.SuperAdmin);
            if (superAdminRole == null)
            {
                superAdminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.SuperAdmin, StaticRoleNames.Tenants.SuperAdmin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Surgeon role

            var surgeonRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Surgeon);
            if (surgeonRole == null)
            {
                surgeonRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Surgeon, StaticRoleNames.Tenants.Surgeon) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Surgeon role

            var anaesthetistsRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Anaesthetist);
            if (anaesthetistsRole == null)
            {
                anaesthetistsRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Anaesthetist, StaticRoleNames.Tenants.Anaesthetist) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var manufacturerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Manufacturer);
            if (manufacturerRole == null)
            {
                manufacturerRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Manufacturer, StaticRoleNames.Tenants.Manufacturer) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant admin specficic permissions to admin role

            string[] superAdminPermissions = new string[] { 
                PermissionNames.Pages_Dashboard,
                PermissionNames.Pages_Roles,
                PermissionNames.Pages_Users,
                PermissionNames.Pages_Surveys,
                PermissionNames.Pages_Poaps,
                PermissionNames.Pages_Theaters,
                PermissionNames.Pages_Settings,
                PermissionNames.Pages_Region_Management,
                PermissionNames.Pages_Hospital_Management,
                PermissionNames.Pages_Device_Management
            };
            GrantPermissions(superAdminRole, superAdminPermissions);

            string[] adminRolePermissions = new string[] {
                PermissionNames.Pages_Users,
                PermissionNames.Pages_Surveys,
                PermissionNames.Pages_Poaps,
                PermissionNames.Pages_Region_Management,
                PermissionNames.Pages_Hospital_Management,
                PermissionNames.Pages_Device_Management,
            };

            GrantPermissions(adminRole, adminRolePermissions);

            // Grant surgeon specficic permissions to surgeon role

            string[] surgeonPermissions = new string[] { 
                PermissionNames.Pages_Dashboard,
                PermissionNames.Pages_Surveys,
                PermissionNames.Pages_Poaps,
            };
            GrantPermissions(surgeonRole, surgeonPermissions);

            // Grant surgeon specficic permissions to anaesthetists role

            string[] anaesthetistsPermissions = new string[] {
                PermissionNames.Pages_Dashboard,
                PermissionNames.Pages_Surveys,
                PermissionNames.Pages_Poaps,
            };
            GrantPermissions(anaesthetistsRole, anaesthetistsPermissions);

            // Grant manufacturer specficic permissions to manufacturers role

            string[] manufacturersPermissions = new string[] {
                PermissionNames.Pages_Devices
            };
            GrantPermissions(manufacturerRole, manufacturersPermissions);

            var hospitalIds = new List<string> { "test", "RX1PH" };

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, superAdminRole.Id));
                    
                // Assign Hospitals to admin user
                foreach (var hospitalId in hospitalIds)
                {
                    _context.UserHospitals.Add(new Entities.UserHospital()
                    {
                        UserId = adminUser.Id,
                        HospitalId = hospitalId,
                    });
                }

                _context.SaveChanges();
            }

            // Admin test users

            var testAdminUsers = new List<User>()
            {
                new User()
                {
                    Name = "Megan",
                    Surname = "Langley",
                    EmailAddress = "megan.langley@sourcecloud.co.uk",
                },
                new User()
                {
                    Name = "Rob",
                    Surname = "Langley",
                    EmailAddress = "rob.langley@sourcecloud.co.uk",
                },
            };
            foreach (var testAdminUser in testAdminUsers)
            {
                var user = _context.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefault(u => u.TenantId == _tenantId && u.EmailAddress == testAdminUser.EmailAddress);
                if (user == null)
                {
                    user = User.CreateTenantAdminUser(_tenantId, testAdminUser.EmailAddress);
                    user.UserName = testAdminUser.EmailAddress;
                    user.Name = testAdminUser.Name;
                    user.Surname = testAdminUser.Surname;
                    user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
                    user.IsEmailConfirmed = true;
                    user.IsActive = true;

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    // Assign Admin role to test admin user
                    _context.UserRoles.Add(new UserRole(_tenantId, user.Id, superAdminRole.Id));
                    
                    // Assign Hospitals to test admin user
                    foreach (var hospitalId in hospitalIds)
                    {
                        _context.UserHospitals.Add(new Entities.UserHospital()
                        {
                            UserId = user.Id,
                            HospitalId = hospitalId,
                        });
                    }

                    _context.SaveChanges();
                }
            }

            // Surgeon test users

            var testSurgeonUsers = new List<User>()
            {
                new User()
                {
                    Name = "Megz",
                    Surname = "Langley",
                    EmailAddress = "megan.langley@me.com",
                },
            };
            foreach (var testSurgeonUser in testSurgeonUsers)
            {
                var user = _context.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefault(u => u.TenantId == _tenantId && u.EmailAddress == testSurgeonUser.EmailAddress);
                if (user == null)
                {
                    user = User.CreateTenantAdminUser(_tenantId, testSurgeonUser.EmailAddress);
                    user.UserName = testSurgeonUser.EmailAddress;
                    user.Name = testSurgeonUser.Name;
                    user.Surname = testSurgeonUser.Surname;
                    user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
                    user.IsEmailConfirmed = true;
                    user.IsActive = true;

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    // Assign Surgeon role to test surgeon user
                    _context.UserRoles.Add(new UserRole(_tenantId, user.Id, surgeonRole.Id));

                    // Assign Hospitals to test surgeon user
                    foreach (var hospitalId in hospitalIds)
                    {
                        _context.UserHospitals.Add(new Entities.UserHospital()
                        {
                            UserId = user.Id,
                            HospitalId = hospitalId,
                        });
                    }
                    
                    _context.SaveChanges();
                }
            }
        }
    }
}
