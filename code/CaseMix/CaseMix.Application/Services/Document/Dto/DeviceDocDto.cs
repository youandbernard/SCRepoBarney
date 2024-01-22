using System;

namespace CaseMix.Services.Document.Dto
{
    public class DeviceDocDto
    {
        public Guid? ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
        public string SpecialtyName { get; set; }
        public string DeviceClassName { get; set; }
        public string DeviceFamilyName { get; set; }
        public int? DocFileId { get; set; }
    }
}
