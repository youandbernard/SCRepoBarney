using Abp.Application.Services;
using CaseMix.Services.IntegratedCareSystems.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.IntegratedCareSystems
{
    public interface IIntegratedCareSystemsAppService : IApplicationService
    {
        Task<IEnumerable<IntegratedCareSystemDto>> GetAll();
    }
}
