using Abp.Application.Services;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.Hospitals
{
    public interface IHospitalsAppService : IAsyncCrudAppService<HospitalDto, string, PagedHospitalResultRequestDto>
    {
        Task<IEnumerable<HospitalDto>> GetByUser(long id);
        Task<HospitalDto> GetByHospitalId(string id);
        Task<IEnumerable<RegionManagementNodeDto>> GetAllHospitalsMultiSelect(long? userId = null);
    }
}
