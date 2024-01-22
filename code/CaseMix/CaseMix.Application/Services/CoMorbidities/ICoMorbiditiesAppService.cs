using Abp.Application.Services;
using CaseMix.Services.CoMorbidities.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.CoMorbidities
{
    public interface ICoMorbiditiesAppService : IApplicationService
    {
        Task<IEnumerable<CoMorbidityGroupDto>> GetAll();
    }
}
