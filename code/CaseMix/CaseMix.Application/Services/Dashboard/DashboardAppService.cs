using Abp.Configuration;
using CaseMix.Aws.OpenSearch.Model;
using CaseMix.Aws.OpenSearch.Services;
using CaseMix.Configuration;
using CaseMix.Entities;
using CaseMix.Services.Dashboard.DataHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.Dashboard
{
    public class DashboardAppService : IDashboardAppService
    {
        private UploadDataHelper _uploadDataHelper;
        private readonly IOpenSearchService _openSearcService;
        private readonly ISettingManager _settingManager;

        private string _openSearchEndpoint = string.Empty;
        private string _masterUser = string.Empty;
        private string _masterPassword = string.Empty;

        public DashboardAppService(
            IOpenSearchService openSearchService,
            ISettingManager settingManager
        )
        {
            _settingManager = settingManager;
            _openSearcService = openSearchService;

            _openSearchEndpoint = _settingManager.GetSettingValue(AppSettingNames.Aws_OpenSearchDomain);
            _masterUser = _settingManager.GetSettingValue(AppSettingNames.Aws_MasterUser);
            _masterPassword = _settingManager.GetSettingValue(AppSettingNames.Aws_MasterPassword);

            _uploadDataHelper = new UploadDataHelper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<OpenSearchResponseDto> UploadHospitalAsync(Hospital hospital)
        {
            OpenSearchResponseDto result = new OpenSearchResponseDto();

            _openSearchEndpoint += "/_bulk";
            
            if (hospital != null)
            {                
                var convertedData = _uploadDataHelper.PrepareDataHospital(hospital);
                result = await _openSearcService.UploadAsync(_openSearchEndpoint, _masterUser, _masterPassword, convertedData);
            }
            else
            {
                result.Success = false;
                result.Errors = new List<string>();
                result.Errors.Add("No data to upload.");
            }

            return result;

        }
    }
}
