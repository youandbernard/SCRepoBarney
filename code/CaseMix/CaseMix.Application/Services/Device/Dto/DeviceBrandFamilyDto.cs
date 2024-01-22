using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    public class DeviceBrandFamilyDto
    {
        public string UniqueID { get; set; }
        public string BrandName { get; set; }
        public int? DeviceFamilyId { get; set; }
        public string DeviceFamilyName { get; set; }
        public Guid? ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
    }

    public class DeviceBrandFamilyViewDto
    {
        public bool IsLicensed { get; set; }
        public IEnumerable<DeviceBrandFamilyDto> DeviceBrandFamilies { get; set; }
    }
}
