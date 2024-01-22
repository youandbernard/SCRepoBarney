using System.Threading.Tasks;
using Abp.Application.Services;
using CaseMix.Sessions.Dto;

namespace CaseMix.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
