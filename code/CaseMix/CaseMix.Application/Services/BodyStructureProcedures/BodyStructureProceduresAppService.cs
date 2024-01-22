using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.BodyStructureProcedures.Dto;
using CaseMix.Services.BodyStructures.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.BodyStructureProcedures
{
    public class BodyStructureProceduresAppService: CaseMixAppServiceBase, IBodyStructureProceduresAppService
    {
        private readonly IRepository<BodyStructureSubProcedure, int> _bodyStructureSubProcedureRepository;
        public BodyStructureProceduresAppService(IRepository<BodyStructureSubProcedure, int> bodyStructureSubProcedureRepository)
        {
            _bodyStructureSubProcedureRepository = bodyStructureSubProcedureRepository;
        }

        public async Task<IEnumerable<BodyStructureSubProcedureDto>> GetAll()
        {
            var ret = await _bodyStructureSubProcedureRepository.GetAll()
                .Include(_ => _.BodyStructure)
                    .ThenInclude(_ => _.BodyStructureGroup)
                .Select(_ => ObjectMapper.Map<BodyStructureSubProcedureDto>(_)).ToListAsync();

            return ret;
        }

    }
}
