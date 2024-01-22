using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using CaseMix.Authorization;
using CaseMix.Authorization.Accounts;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using CaseMix.Entities;
using CaseMix.Roles.Dto;
using CaseMix.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CaseMix.Users
{
    [AbpAllowAnonymous]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserHospital, Guid> _userHospitalsRepository;
        private readonly IRepository<Manufacture, Guid> _manufacturesRepository;
        private readonly IRepository<UserRealmMapping, Guid> _UserRealmMappingRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IRepository<UserHospital, Guid> userHospitalsRepository,
            IRepository<Manufacture, Guid> manufacturesRepository,
            IRepository<UserRealmMapping, Guid> userRealmMappingRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _userHospitalsRepository = userHospitalsRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _manufacturesRepository = manufacturesRepository;
            _UserRealmMappingRepository = userRealmMappingRepository;
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();
            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            if (user.ManufactureId != null)
            {
                var manufacture = _manufacturesRepository.GetAll().Where(e => e.Id == user.ManufactureId).FirstOrDefaultAsync();
                userDto.ManufactureName = manufacture.Result.Name;
            }
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            try
            {
                long userId = _abpSession.UserId.Value;
                var currentUser = _userManager.GetUserByIdAsync(userId);
                User user = currentUser.Result;

                Task<List<UserHospital>> allUserHospitalsQuery = null;
                allUserHospitalsQuery = Task.Run(async () => await GetAllUserHospitals());

                var allUserHospitals = allUserHospitalsQuery.Result;

                List<long> users = new List<long>();
                if (allUserHospitals != null)
                    users = allUserHospitals.Where(_ => _.HospitalId == input.HospitalId).Select(e => e.UserId).ToList();

                if (user != null)
                {
                    var roles = _userManager.GetRolesAsync(user);
                    if (roles.Result.Contains(StaticRoleNames.Tenants.SuperAdmin))
                    {

                        var dataSAdmin = Repository.GetAll()
                        .Include(e => e.Roles)
                        .Include(e => e.Manufacture)
                        .Include(e => e.UserRealmMappings)
                            .ThenInclude(e => e.Region)
                        .Include(e => e.UserRealmMappings)
                            .ThenInclude(e => e.Hospital)
                        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                            x.UserName.ToLower().Contains(input.Keyword.ToLower()) ||
                            x.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                            x.EmailAddress.ToLower().Contains(input.Keyword.ToLower()) ||
                            x.Id.ToString().ToLower().Contains(input.Keyword.ToLower())
                        )
                        .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);

                        return dataSAdmin;
                    }
                    else
                    {
                        List<string> hospitalIds = new List<string>();
                        if (allUserHospitals != null)
                            hospitalIds = allUserHospitals.Where(_ => _.UserId == userId).Select(_ => _.HospitalId).ToList(); ;

                        if (hospitalIds != null)
                        {
                            if (hospitalIds.Count > 0)
                            {
                                List<long> allUsersHospital = new List<long>();

                                if (allUserHospitals != null)
                                    allUsersHospital = allUserHospitals.Where(_ => hospitalIds.Any(x => x == _.HospitalId)).Select(_ => _.UserId).ToList();

                                if (allUsersHospital != null)
                                {
                                    foreach (var mUser in allUsersHospital)
                                    {
                                        users.Add(mUser);
                                    }
                                }
                            }
                        }

                        var manufacturerUsers = Task.Run(async () => await GetAllUserManufacturers());

                        if (manufacturerUsers.Result != null)
                        {
                            foreach (var mUser in manufacturerUsers.Result)
                            {
                                if (mUser != null)
                                {
                                    users.Add(mUser.Id);
                                }
                            }
                        }
                    }
                }

                var data = Repository.GetAll()
                    .Include(e => e.Roles)
                    .Include(e => e.Manufacture)
                    .Include(e => e.UserRealmMappings)
                        .ThenInclude(e => e.Region)
                    .Include(e => e.UserRealmMappings)
                        .ThenInclude(e => e.Hospital)
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => 
                        x.UserName.ToLower().Contains(input.Keyword.ToLower()) || 
                        x.Name.ToLower().Contains(input.Keyword.ToLower()) || 
                        x.EmailAddress.ToLower().Contains(input.Keyword.ToLower()) ||
                        x.Id.ToString().ToLower().Contains(input.Keyword.ToLower())
                    )
                    .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
                    .Where(e => users.Any(x => x == e.Id));

                return data;
            }
            catch (UserFriendlyException ex)
            {
                return null;
            }

        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        [AbpAllowAnonymous]
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.SuperAdmin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }

        [AbpAllowAnonymous]
        public async Task<IEnumerable<UserDto>> GetSurgeons(string keyword, string hospitalId)
        {
            var surgeonRole = await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.Surgeon);
            var surgeons = await _userHospitalsRepository.GetAll()
                .WhereIf(!hospitalId.IsNullOrWhiteSpace(), e => e.HospitalId == hospitalId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), e => e.User.Name.ToLower().Contains(keyword.ToLower())
                    || e.User.Surname.ToLower().Contains(keyword.ToLower()))
                .Where(e => e.User.Roles.Any(r => r.RoleId == surgeonRole.Id))
                .Select(e => e.User)
                .Distinct()
                .Take(10)
                .Select(e => ObjectMapper.Map<UserDto>(e))
                .ToListAsync();

            return surgeons;
        }

        [AbpAllowAnonymous]
        public async Task<IEnumerable<UserDto>> GetAnaesthetists(string keyword, string hospitalId)
        {
            var surgeonRole = await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.Anaesthetist);
            var anaesthestists = await _userHospitalsRepository.GetAll()
                .WhereIf(!hospitalId.IsNullOrWhiteSpace(), e => e.HospitalId == hospitalId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), e => e.User.Name.ToLower().Contains(keyword.ToLower())
                    || e.User.Surname.ToLower().Contains(keyword.ToLower()))
                .Where(e => e.User.Roles.Any(r => r.RoleId == surgeonRole.Id))
                .Select(e => e.User)
                .Distinct()
                .Take(10)
                .Select(e => ObjectMapper.Map<UserDto>(e))
                .ToListAsync();

            return anaesthestists;
        }

        [AbpAllowAnonymous]
        public async Task<IEnumerable<UserDto>> GetAllSurgeons(string hospitalId)
        {
            var surgeonRole = await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.Surgeon);
            var surgeons = await _userHospitalsRepository.GetAll()
                .WhereIf(!hospitalId.IsNullOrWhiteSpace(), e => e.HospitalId == hospitalId)
                .Where(e => e.User.Roles.Any(r => r.RoleId == surgeonRole.Id))
                .Select(e => e.User)
                .Distinct()
                .Select(e => ObjectMapper.Map<UserDto>(e))
                .ToListAsync();

            return surgeons;
        }

        [AbpAllowAnonymous]
        public async Task<IEnumerable<UserDto>> GetAllAnaesthetists(string hospitalId)
        {
            var surgeonRole = await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.Anaesthetist);
            var anaesthestists = await _userHospitalsRepository.GetAll()
                .WhereIf(!hospitalId.IsNullOrWhiteSpace(), e => e.HospitalId == hospitalId)
                .Where(e => e.User.Roles.Any(r => r.RoleId == surgeonRole.Id))
                .Select(e => e.User)
                .Distinct()
                .Select(e => ObjectMapper.Map<UserDto>(e))
                .ToListAsync();

            return anaesthestists;
        }

        [AbpAllowAnonymous]
        public async Task<bool> GetAllUserLoggedInRoles()
        {
            long userId = _abpSession.UserId.Value;
            var roles = await GetLoggedInUserRoles(userId);
            var isAdmin = false;
            if (roles.Any(t => t == "ADMIN"))
            {
                isAdmin = true;
            }
            return isAdmin;
        }

        private async Task<IQueryable<string>> GetLoggedInUserRoles(long userId)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == userId);
            var roleIds = user.Roles.Select(e => e.RoleId);
            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            return roles;
        }

        private async Task<List<UserHospital>> GetAllUserHospitals()
        {
            try
            {
                //await _userHospitalsRepository.dis
                var users = await _userHospitalsRepository.GetAll().AsNoTracking()
                .ToListAsync();

                return users;
            }
            catch (UserFriendlyException ex)
            {
                return new List<UserHospital>();
            }
            catch (AggregateException ex)
            {
                return new List<UserHospital>();
            }
        }


        private async Task<List<User>> GetAllUserManufacturers()
        {
            var manufacturerRoledId = await _roleManager.GetRoleByNameAsync("MANUFACTURER");
            if (manufacturerRoledId != null)
            {
                var result = await Repository.GetAll()
                    .Where(_ => _.Roles.Select(r => r.RoleId).Contains(manufacturerRoledId.Id)).ToListAsync();

                return result;
            }

            return null;
        }
    }
}

