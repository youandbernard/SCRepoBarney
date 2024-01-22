using Abp.Auditing;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    public class FileDto
    {
        public long? DocumentId { get; set; }
        public string ManufacturerId { get; set; }
        public string BodyStructureGroupId { get; set; }
        public string DeviceClassId { get; set; }
        public int? DeviceFamilyId { get; set; }

        [DisableAuditing]
        public IFormFile File { get; set; }
    }
}
