using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.RiskMappingSettings.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.RiskMappingSettings
{
   public  class RiskMappingAppService : CaseMixAppServiceBase, IRiskMappingAppService
    {
        private readonly IRepository<RiskMappingSetting, int> _riskMappingSettingRepository;
        
        public RiskMappingAppService(IRepository<RiskMappingSetting, int> riskMappingSettingRepository)
        {
            _riskMappingSettingRepository = riskMappingSettingRepository;
        }

        public async Task<IEnumerable<RiskMappingSettingDto>> GetAll()
        {
            var settings = await _riskMappingSettingRepository.GetAll()
                .Include(e => e.Hospital)
                .Select(e => ObjectMapper.Map<RiskMappingSettingDto>(e))
                .ToListAsync();

            return settings;
        }

        public async Task SaveAll(IEnumerable<RiskMappingSettingDto> inputs)
        {
            foreach (var input in inputs)
            {
                var isExisting = _riskMappingSettingRepository.GetAll()
                    .Where(e => e.HospitalId == input.HospitalId)
                    .FirstOrDefault();

                if (isExisting != null)
                {
                    isExisting.IsEnabled = input.IsEnabled;
                    await _riskMappingSettingRepository.UpdateAsync(isExisting);
                }
                else
                {
                    var setting = ObjectMapper.Map<RiskMappingSetting>(input);
                    await _riskMappingSettingRepository.InsertAsync(setting);
                }
            }
        }

        public async Task<bool> GetRiskMappingSettingByHospital(string hospitalId)
        {
            var hospital = await _riskMappingSettingRepository.FirstOrDefaultAsync(e => e.HospitalId == hospitalId);
            if (hospital != null)
            {
                return hospital.IsEnabled;
            }

            return false;
        }
    }
}
