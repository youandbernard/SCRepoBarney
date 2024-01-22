using Abp.Application.Services.Dto;
using AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.Hospitals.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.RiskMappingSettings.Dto
{
    [AutoMap(typeof(RiskMappingSetting))]
    public class RiskMappingSettingDto : EntityDto<int>
    {
        public string HospitalId { get; set; }
        public string HospitalName { get; set; }
        public bool IsEnabled { get; set; }
        public HospitalDto Hospital { get; set; }
    }
}
