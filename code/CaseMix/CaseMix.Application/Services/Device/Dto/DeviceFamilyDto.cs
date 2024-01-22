using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    [AutoMap(typeof(CaseMix.Entities.DeviceFamily))]
    public class DeviceFamilyDto : EntityDto<int>
    {
        public Guid DeviceFamilyId { get; set; }

        public string Name { get; set; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }

        public Guid? BodyStructureGroupId { get; set; }
    }
}
