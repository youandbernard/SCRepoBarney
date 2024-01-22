using Abp.Application.Services;
using CaseMix.Services.BodyStructures.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.BodyStructures
{
    public interface IBodyStructuresAppService : IApplicationService
    {
        Task<IEnumerable<BodyStructureDto>> GetAll();
        IEnumerable<ProcedureMethodDto> GetMethods();
        Task<IEnumerable<MenuItemOutputDto>> GetMenu(int id);
        Task RefreshMenuCache(int id);
        Task ClearMenuCache();
    }
}
