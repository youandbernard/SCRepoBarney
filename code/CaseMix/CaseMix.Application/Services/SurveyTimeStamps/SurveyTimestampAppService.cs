using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.SurveyTimeStamps.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CaseMix.Services.SurveyTimeStamps
{
    public class SurveyTimestampAppService: CaseMixAppServiceBase, ISurveyTimestampAppService
    {
        private readonly IRepository<SurveyTimestampSetting, int> _surveyTimestampSettingRepository;

        public SurveyTimestampAppService(IRepository<SurveyTimestampSetting, int> surveyTimestampSettingRepository)
        {
            _surveyTimestampSettingRepository = surveyTimestampSettingRepository;
        }

        public async Task<IEnumerable<SurveyTimestampSettingDto>> GetAll()
        {
            var settings = await _surveyTimestampSettingRepository.GetAll()
                .Include(e => e.Hospital)
                .Select(e => ObjectMapper.Map<SurveyTimestampSettingDto>(e))
                .ToListAsync();

            return settings;
        }

        public async Task SaveAll(IEnumerable<SurveyTimestampSettingDto> inputs)
        {
            foreach(var input in inputs)
            {
                var isExisting =  _surveyTimestampSettingRepository.GetAll()
                    .Where(e => e.HospitalId == input.HospitalId)
                    .FirstOrDefault();

                if(isExisting != null)
                {
                    isExisting.IsEnabled = input.IsEnabled;
                    await _surveyTimestampSettingRepository.UpdateAsync(isExisting);
                } 
                else
                {
                    var setting = ObjectMapper.Map<SurveyTimestampSetting>(input);
                    await _surveyTimestampSettingRepository.InsertAsync(setting);
                }
            }
        }
    }
}
