using Abp.Application.Services;
using CaseMix.Services.TrustIcsMappings.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.TrustIcsMappings
{
    public interface ITrustIcsMappingsAppService : IApplicationService
    {
        Task<IEnumerable<TrustIcsMappingDto>> GetAll();
    }
}
