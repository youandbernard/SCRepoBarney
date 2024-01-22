using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.Manufactures.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    [AutoMap(typeof(CaseMix.Entities.Device))]
    public class DeviceDto: EntityDto<int>
    {
        public string UID { get; set; }
        public string GMDNTermCode { get; set; }
        public string DeviceName { get; set; }
        public string DeviceDescription { get; set; }
        public string BrandName { get; set; }
        public string Model { get; set; }
        public Guid? ManufacturerId { get; set; }
        public int? DocFileId { get; set; }
        public int Status { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? BodyStructureGroupId { get; set; }
        public string DeviceLookupDesc { get; set; }
        public Guid? DeviceClassId { get; set; }
        public int? DeviceFamilyId { get; set; }
        public string GS1Code { get; set; }
        public long? GTINCode { get; set; }

        [NotMapped]
        public string SpecialtyName { get; set; }

        [NotMapped]
        public string StatusName { get; set; }

        [NotMapped]
        public bool IsAvailable { get; set; } = true;

        [NotMapped]
        public string DeviceClassName { get; set; }

        [NotMapped]
        public string ManufacturerName { get; set; }

        [NotMapped]
        public bool SuperAdminUser { get; set; } = false;

        public virtual ManufactureDto Manufacturer { get; set; }

        public virtual BodyStructureGroupDto BodyStructureGroup { get; set; }

        public virtual DeviceClassDto DeviceClass { get; set; }

        public virtual DeviceFamilyDto DeviceFamily { get; set; }
    }
}
