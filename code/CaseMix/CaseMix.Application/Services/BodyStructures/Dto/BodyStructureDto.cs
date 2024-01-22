using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.BodyStructureProcedures.Dto;
using System.Collections.Generic;

namespace CaseMix.Services.BodyStructures.Dto
{
    [AutoMap(typeof(BodyStructure))]
    public class BodyStructureDto : EntityDto<int>
    {
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        public BodyStructureGroupDto BodyStructureGroup { get; set; }
        public IEnumerable<BodyStructureQueryDto> BodyStructureQueries { get; set; }
        public IList<BodyStructureSubProcedureDto> BodyStructureSubProcedures { get; set; }
        
    }
}
