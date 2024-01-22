using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.BodyStructures.Dto
{
    [AutoMap(typeof(BodyStructureQuery))]
    public class BodyStructureQueryDto: EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Query { get; set; }
        public string QuerySimplified { get; set; }
        public int QueryOrder { get; set; }
        public int BodyStructureId { get; set; }

        public BodyStructureDto BodyStructure { get; set; }
    }
}
