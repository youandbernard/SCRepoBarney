using Abp.Application.Services;
using CaseMix.Services.UkRegions.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.UkRegions
{
    public interface IUkRegionsAppService : IApplicationService
    {
        Task<IEnumerable<UkRegionDto>> GetAll();
    }
}
