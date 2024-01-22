using Abp.Application.Services;
using CaseMix.Services.Ethnicities.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.Ethnicities
{
    public interface IEthnicitiesAppService : IApplicationService
    {
        Task<IEnumerable<EthnicityDto>> GetAll();
    }
}
