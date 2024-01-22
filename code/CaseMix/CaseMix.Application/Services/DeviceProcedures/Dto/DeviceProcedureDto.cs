using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.BodyStructureProcedures.Dto;
using CaseMix.Services.BodyStructures.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.DeviceProcedures.Dto
{
    [AutoMap(typeof(CaseMix.Entities.DeviceProcedure))]
    public class DeviceProcedureDto: EntityDto<int>
    {
        public int DeviceId { get; set; }
        public Guid BodyStructureGroupId { get; set; }
        public int? BodyStructureId { get; set; }
        public int? BodyStructureProcId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SnomedID { get; set; }
        public string HospitalId { get; set; }

        public virtual BodyStructureGroupDto BodyStructureGroup { get; set; }
        public virtual BodyStructureDto BodyStructure { get; set; }
        public virtual BodyStructureSubProcedureDto BodyStructureSubProcedure { get; set; }
    }
}
