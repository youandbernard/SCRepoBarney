using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.BodyStructures.Dto;
using CaseMix.Services.UserSpecialties.Dto;
using System;
using System.Collections.Generic;

namespace CaseMix.Services.BodyStructureGroups.Dto
{
    [AutoMap(typeof(BodyStructureGroup))]
    public class BodyStructureGroupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        public IEnumerable<BodyStructureDto> BodyStructures { get; set; }
        public IEnumerable<SurgeonSpecialtyDto> SurgeonSpecialties { get; set; }
    }
}
