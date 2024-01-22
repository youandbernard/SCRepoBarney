using AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.Hospitals.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.SurveyTimeStamps.Dto
{
    [AutoMap(typeof(SurveyTimestampSetting))]
    public class SurveyTimestampSettingDto
    {
        public int Id { get; set; }
        public string HospitalId { get; set; }
        public string HospitalName { get; set; }
        public bool IsEnabled { get; set; }
        public HospitalDto Hospital { get; set; }
    }
}
