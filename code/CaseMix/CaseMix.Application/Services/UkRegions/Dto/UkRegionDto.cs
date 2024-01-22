using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.UkRegions.Dto
{
    [AutoMap(typeof(UkRegion))]
    public class UkRegionDto : EntityDto<int>
    {
        public string Name { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
