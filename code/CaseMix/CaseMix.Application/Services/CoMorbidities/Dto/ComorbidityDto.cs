using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.CoMorbidities.Dto
{
    [AutoMap(typeof(Comorbidity))]
    public class ComorbidityDto
    {
        public string SnomedId { get; set; }
        public string Description { get; set; }
    }
}
