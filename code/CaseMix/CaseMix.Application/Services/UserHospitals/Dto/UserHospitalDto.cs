using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.Hospitals.Dto;
using System;

namespace CaseMix.Services.UserHospitals.Dto
{
    [AutoMap(typeof(UserHospital))]
    public class UserHospitalDto : EntityDto<Guid?>
    {
        public long UserId { get; set; }
        public string HospitalId { get; set; }
        public bool IsSelected { get; set; }
        public HospitalDto Hospital { get; set; }
    }
}
