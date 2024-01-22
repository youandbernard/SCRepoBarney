using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CaseMix.Services.CoMorbidities.Dto
{
    [AutoMap(typeof(ComorbidityGroup))]
    public class CoMorbidityGroupDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<ComorbidityDto> Comorbidities { get; set; }
        public string SnomedId { get; set; }
    }
}
