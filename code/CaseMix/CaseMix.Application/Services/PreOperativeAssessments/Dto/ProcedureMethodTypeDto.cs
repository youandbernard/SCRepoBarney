using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PreOperativeAssessments.Dto
{
    [AutoMap(typeof(ProcedureMethodType))]
    public class ProcedureMethodTypeDto : EntityDto<Guid>
    {
        public string Description { get; set; }
    }
}
