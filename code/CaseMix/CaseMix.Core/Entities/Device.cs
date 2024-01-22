using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("devices")]
    public class Device: Entity<int>
    {
        public string UID { get; set; }
        public string GMDNTermCode { get; set; }
        public string DeviceName { get; set; }
        public string DeviceDescription { get; set; }
        public string BrandName { get; set; }
        public string Model { get; set; }
        public Guid ManufacturerId { get; set; }
        public int? DocFileId { get; set; }
        public int Status { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }   
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? BodyStructureGroupId { get; set; }
        public Guid? DeviceClassId { get; set; }
        public int? DeviceFamilyId { get; set; }
        public string GS1Code { get; set; }
        public long? GTINCode { get; set; }

        [NotMapped]
        public string SpecialtyName { get; set; }

        [NotMapped]
        public string ManufacturerName { get; set; }

        [NotMapped]
        public string StatusName { get; set; }

        [NotMapped]
        public bool IsAvailable { get; set; } = true;

        [NotMapped]
        public string DeviceFamilyName { get; set; }

        [NotMapped]
        public string DeviceClassName { get; set; }

        [NotMapped]
        public bool SuperAdminUser { get; set; } = false;

        [ForeignKey("ManufacturerId")]
        public virtual Manufacture Manufacturer { get; set; }

        [ForeignKey("BodyStructureGroupId")]
        public virtual BodyStructureGroup BodyStructureGroup { get; set; }

        [ForeignKey("DeviceClassId")]
        public virtual DeviceClass DeviceClass { get; set; }

        [ForeignKey("DeviceFamilyId")]
        public virtual DeviceFamily DeviceFamily { get; set; }
    }
}
