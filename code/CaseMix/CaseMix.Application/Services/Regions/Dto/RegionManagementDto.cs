using CaseMix.Services.IntegratedCareSystems.Dto;
using CaseMix.Services.TrustIcsMappings.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Regions.Dto
{
    public class RegionManagementDto
    {
        public IEnumerable<RegionManagementNodeDto> Regions { get; set; }
    }


    public class RegionManagementNodeDto
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string StyleClass { get; set; }
        public bool Expanded { get; set; } = true;
        public string Icon { get; set; }
        public bool? PartialSelected { get; set; } = false;
        public bool? Selected { get; set; } = false;
        public RegionManagementDataDto Data { get; set; } 
        public List<RegionManagementNodeDto> Children { get; set; }
    }

    public class RegionManagementDataDto
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string DataType { get; set; }
        public string ParentId { get; set; }
        public string TrustId { get; set; }
        public string Postcode { get; set; }
        public string TrustName { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public int? IcsId { get; set; }
        public string GroupTrust { get; set; }
        public bool activeDevMgt { get; set; }
        public bool ShowButtonDevProc { get; set; }
        public List<IntegratedCareSystemDto> SelectedIcs { get; set; }
    }
}
