using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    [AutoMap(typeof(CaseMix.Entities.DeviceClass))]
    public class DeviceClassDto: EntityDto<Guid>
    {
        public string Class { get; set; }

        public string Rule { get; set; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }
    }
}
