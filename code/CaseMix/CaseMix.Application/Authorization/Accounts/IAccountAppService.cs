using System.Threading.Tasks;
using Abp.Application.Services;
using CaseMix.Authorization.Accounts.Dto;

namespace CaseMix.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
