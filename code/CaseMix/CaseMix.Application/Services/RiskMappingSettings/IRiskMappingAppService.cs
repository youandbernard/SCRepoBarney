using CaseMix.Services.RiskMappingSettings.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.RiskMappingSettings
{
    public interface IRiskMappingAppService
    {
        Task<IEnumerable<RiskMappingSettingDto>> GetAll();
        Task SaveAll(IEnumerable<RiskMappingSettingDto> inputs);
        Task<bool> GetRiskMappingSettingByHospital(string hospitalId);
    }
}
