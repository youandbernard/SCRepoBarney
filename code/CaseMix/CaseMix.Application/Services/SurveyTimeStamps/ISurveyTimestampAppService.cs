using Abp.Application.Services;
using CaseMix.Services.SurveyTimeStamps.Dto;
using CaseMix.Services.UserSpecialties.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.SurveyTimeStamps
{
    public interface ISurveyTimestampAppService : IApplicationService
    {
        
        Task<IEnumerable<SurveyTimestampSettingDto>> GetAll();
        Task SaveAll(IEnumerable<SurveyTimestampSettingDto> inputs);
    }
}
