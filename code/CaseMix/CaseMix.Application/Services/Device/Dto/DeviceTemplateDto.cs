using System;

namespace CaseMix.Services.Device.Dto
{
    public class DeviceTemplateDto
    {
        public string UDI { get; set; }
        public string GMDNTermCode { get; set; }
        public string DeviceName { get; set; }
        public string DeviceDescription { get; set; }
        public string BrandName { get; set; }
        public string Model { get; set; }
        public Guid Specialty { get; set; }
        public int DeviceFamily { get; set; }
        public Guid Class { get; set; }
        public long GTINCode { get; set; }
    }
}
