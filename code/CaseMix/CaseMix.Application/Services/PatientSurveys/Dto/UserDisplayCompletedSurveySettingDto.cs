using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PatientSurveys.Dto
{
    [AutoMap(typeof(UserDisplayCompletedSurveySetting))]
    public class UserDisplayCompletedSurveySettingDto : EntityDto<int>
    {
        public long UserId { get; set; }
        public bool DisplayCompletedSurvey { get; set; }
    }
}
