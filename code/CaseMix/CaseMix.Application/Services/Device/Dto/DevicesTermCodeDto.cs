using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    public class DevicesTermCodeDto
    {
        public string GMDNTermCode { get; set; }
        public string DeviceName { get; set; }
        public string DeviceDescription { get; set; }
        public string BrandName { get; set; }
        public Guid? ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }

        public string GMDNDefinition { get; set; }
        public string GMDNStatus { get; set; }
        public string GMDNTermIsIVD { get; set; }
        public string GMDNCreatedDate { get; set; }
        public string GMDNObsoletedDate { get; set; }
    }
}
