using Abp.Application.Services;
using CaseMix.Services.Manufactures.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.Manufactures
{
    public interface IManufacturesAppService : IApplicationService
    {
        Task<IEnumerable<ManufactureDto>> GetAll();
    }
}
