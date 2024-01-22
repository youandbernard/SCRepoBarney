using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Regions.Dto
{
    [AutoMap(typeof(Region))]
    public class RegionDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid? ParentId { get; set; }
        public virtual bool IsEnabled { get; set; }
    }


    public class RegionHospitalMappingDto
    {
        public List<string> HospitalIds { get; set; }
        public List<Guid> RegionIds { get; set; }
        public long UserId { get; set; }
    }
}
