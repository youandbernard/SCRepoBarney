using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CaseMix.Roles.Dto;
using CaseMix.Users.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);

        Task<IEnumerable<UserDto>> GetSurgeons(string keyword, string hospitalId);
        Task<IEnumerable<UserDto>> GetAnaesthetists(string keyword, string hospitalId);
        Task<IEnumerable<UserDto>> GetAllSurgeons(string hospitalId);

        Task<IEnumerable<UserDto>> GetAllAnaesthetists(string hospitalId);
        Task<bool> GetAllUserLoggedInRoles();

    }
}
