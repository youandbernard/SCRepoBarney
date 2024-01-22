using Abp.Application.Services;
using CaseMix.Services.UserHospitals.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.UserHospitals
{
    public interface IUserHospitalsAppService : IApplicationService
    {
        Task<IEnumerable<UserHospitalDto>> GetAll(long userId);
        Task SaveAll(IEnumerable<UserHospitalDto> inputs);
    }
}
