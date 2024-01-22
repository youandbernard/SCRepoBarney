using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.IntegratedCareSystems.Dto;
using CaseMix.Services.Regions.Dto;
using CaseMix.Services.UkRegions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.TrustIcsMappings.Dto
{
    [AutoMap(typeof(TrustIcsMapping))]
    public class TrustIcsMappingDto : EntityDto<Guid>
    {
        public Guid? RegionId { get; set; }
        public int IntegratedCareSystemId { get; set; }
        public RegionDto Region { get; set; }
        public IntegratedCareSystemDto IntegratedCareSystem { get; set; }
    }
}
