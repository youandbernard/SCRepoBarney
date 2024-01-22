using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.UkRegions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.IntegratedCareSystems.Dto
{
    [AutoMap(typeof(IntegratedCareSystem))]
    public class IntegratedCareSystemDto : EntityDto<int>
    {
        public string Name { get; set; }
        public int UkRegionId { get; set; }
        public UkRegionDto UkRegion { get; set; }
    }
}
