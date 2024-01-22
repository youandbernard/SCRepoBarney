using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Device.Dto
{
    public class PagedDeviceResultRequestDto: PagedAndSortedResultRequestDto
    {
        public Guid? ManufacturerId { get; set; }
        public Guid? BodyStructureGroupId { get; set; }
        public Guid? DeviceClassId { get; set; }
        public string hospitalId { get; set; } = string.Empty;
        public bool DisabledOnly { get; set; } = false;
        public bool UnAvailableOnly { get; set; } = false;
        public string Keyword { get; set; }
    }
}
