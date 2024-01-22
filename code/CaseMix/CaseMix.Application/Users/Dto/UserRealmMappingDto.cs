using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Users.Dto
{
    [AutoMapFrom(typeof(UserRealmMapping))]
    public class UserRealmMappingDto : EntityDto<Guid>
    {
        public virtual long? UserId { get; set; }
        public virtual Guid? RegionId { get; set; }
        public virtual string HospitalId { get; set; }

        public UserDto User { get; set; }
        public RegionDto Region { get; set; }
        public HospitalDto Hospital { get; set; }
    }
}
