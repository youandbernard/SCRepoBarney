using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.Countries.Dto;
using CaseMix.Services.Regions.Dto;
using CaseMix.Services.ReportSettings.Dto;
using CaseMix.Services.RiskMappingSettings.Dto;
using CaseMix.Services.Trusts.Dto;
using System;
using System.Collections.Generic;

namespace CaseMix.Services.Hospitals.Dto
{
    [AutoMap(typeof(Hospital))]
    public class HospitalDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string ReportUrl { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? TrustId { get; set; }
        public int? IcsId { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string TrustName { get; set; }
        public string Postcode { get; set; }
        public bool ActiveDevMgt { get; set; }
        public bool? ShowButtonDevProc { get; set; }

        public CountryDto Country { get; set; }
        public RegionDto Region { get; set; }
        public TrustDto Trust { get; set; }
        public ReportingSettingDto Setting { get; set; }
        public RiskMappingSettingDto RiskMappingSetting { get; set; }
        
        public IEnumerable<SpecialtyInfoDto> SpecialtyInfos { get; set; }
    }
}
