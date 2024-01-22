using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    [AutoMap(typeof(CaseMix.Entities.DevicesHospital))]
    public class DevicesHospitalDto : EntityDto<int>
    {
        public int DeviceId { get; set; }

        public string HospitalId { get; set; }

        public long UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual DeviceDto Device { get; set; }
    }
}
