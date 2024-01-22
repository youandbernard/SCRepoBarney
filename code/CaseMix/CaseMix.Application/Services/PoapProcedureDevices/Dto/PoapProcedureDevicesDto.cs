using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Services.Device.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PoapProcedureDevices.Dto
{
    [AutoMap(typeof(CaseMix.Entities.PoapProcedureDevices))]
    public class PoapProcedureDevicesDto: EntityDto<int>
    {
        public Guid PreOperativeAssessmentId { get; set; }
        public Guid PoapProcedureId { get; set; }
        public int DeviceId { get; set; }
        public string SnomedId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual DeviceDto Device { get; set; }
    }
}
