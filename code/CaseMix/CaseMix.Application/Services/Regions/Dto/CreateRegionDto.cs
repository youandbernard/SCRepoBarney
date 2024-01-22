using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Regions.Dto
{
    public class CreateRegionDto 
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid? ParentId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public List<int> IcsIds { get; set; }
    }

}
