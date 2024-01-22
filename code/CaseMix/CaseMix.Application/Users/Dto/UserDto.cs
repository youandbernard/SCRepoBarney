using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using CaseMix.Authorization.Users;
using CaseMix.Entities.Enums;
using CaseMix.Services.Manufactures.Dto;

namespace CaseMix.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }

        public SurgeonExperienceType Experience { get; set; }

        public Guid? ManufactureId { get; set; }
        public string ManufactureName { get; set; }

        public ManufactureDto Manufacture { get; set; }
        public ICollection<UserRealmMappingDto> UserRealmMappings { get; set; }
        public string UserRealmMappingRegions { get; set; }
        public string UserRealmMappingHospitals { get; set; }
    }
}
