using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.ReportSettings.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.ReportSettings
{
    public class ReportingSettingsAppService : CaseMixAppServiceBase, IReportingSettingsAppService
    {
        private readonly IRepository<ReportingSetting, int> _reportingSettingsRepository;

        public ReportingSettingsAppService(IRepository<ReportingSetting, int> reportingSettingsRepository)
        {
            _reportingSettingsRepository = reportingSettingsRepository;
        }

        public async Task<IEnumerable<ReportingSettingDto>> GetAll()
        {
            var settings = await _reportingSettingsRepository.GetAll()
                .Include(e => e.Hospital)
                .Select(e => ObjectMapper.Map<ReportingSettingDto>(e))
                .ToListAsync();

            return settings;
        } 

        public async Task SaveAll(IEnumerable<ReportingSettingDto> inputs)
        {
            foreach (var input in inputs)
            {
                var isExisting = _reportingSettingsRepository.GetAll()
                    .Where(e => e.HospitalId == input.HospitalId)
                    .FirstOrDefault();

                if (isExisting != null)
                {
                    isExisting.IsEnabled = input.IsEnabled;
                    await _reportingSettingsRepository.UpdateAsync(isExisting);
                }
                else
                {
                    var setting = ObjectMapper.Map<ReportingSetting>(input);
                    await _reportingSettingsRepository.InsertAsync(setting);
                }
            }
        }

        public async Task<bool> GetByHospitalReportingSetting(string hospitalId)
        {
            var setting = await _reportingSettingsRepository.FirstOrDefaultAsync(e => e.HospitalId == hospitalId);

            if (setting != null)
                return setting.IsEnabled;

            return false;
        }
    }
}
