using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.BodyStructures.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.BodyStructureProcedures.Dto
{
    [AutoMap(typeof(BodyStructureSubProcedure))]
    public class BodyStructureSubProcedureDto : EntityDto<int>
    {
        public string SnomedId { get; set; }
        public string Description { get; set; }
        public int BodyStructureId { get; set; }
        public bool ShowButtonDevProc { get; set; }

        public BodyStructureDto BodyStructure { get; set; }
    }
}
