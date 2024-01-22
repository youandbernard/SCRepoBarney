using Abp.Application.Services;
using CaseMix.Services.ReportSettings.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.ReportSettings
{
    public interface IReportingSettingsAppService : IApplicationService
    {
        Task<IEnumerable<ReportingSettingDto>> GetAll();
        Task SaveAll(IEnumerable<ReportingSettingDto> inputs);
        Task<bool> GetByHospitalReportingSetting(string hospitalId);
    }
}
