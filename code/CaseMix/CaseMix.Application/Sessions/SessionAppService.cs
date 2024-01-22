using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Domain.Repositories;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using CaseMix.Entities;
using CaseMix.Sessions.Dto;

namespace CaseMix.Sessions
{
    public class SessionAppService : CaseMixAppServiceBase, ISessionAppService
    {
        private readonly IRepository<UserDisplayCompletedSurveySetting> _displayCompletedSurveyRepository;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        public SessionAppService(
                IRepository<UserDisplayCompletedSurveySetting> displayCompletedSurveyRepository,
                UserManager userManager,
                RoleManager roleManager
            )
        {
            _displayCompletedSurveyRepository = displayCompletedSurveyRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                var displayCompletedSurveySetting = await _displayCompletedSurveyRepository.FirstOrDefaultAsync(e => e.UserId == output.User.Id);
                output.User.DisplayCompletedSurvey = displayCompletedSurveySetting == null ? false : displayCompletedSurveySetting.DisplayCompletedSurvey;
                var currentUser = await _userManager.GetUserByIdAsync(output.User.Id);
                var roles = await _userManager.GetRolesAsync(currentUser);
                output.User.IsAdmin = !roles.Contains(StaticRoleNames.Tenants.SuperAdmin) ? false : true;
                output.User.RoleNames = roles;
            }

            return output;
        }
    }
}
