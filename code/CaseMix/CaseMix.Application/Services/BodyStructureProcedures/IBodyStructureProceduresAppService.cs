using Abp.Application.Services;
using CaseMix.Services.BodyStructureProcedures.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.BodyStructureProcedures
{
    public interface IBodyStructureProceduresAppService: IApplicationService
    {
        Task<IEnumerable<BodyStructureSubProcedureDto>> GetAll();
    }
}
