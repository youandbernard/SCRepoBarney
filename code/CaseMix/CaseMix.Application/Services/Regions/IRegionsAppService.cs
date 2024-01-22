using Abp.Application.Services;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.Regions
{
    public interface IRegionsAppService : IApplicationService
    {
        Task<IEnumerable<RegionDto>> GetAll(string countryName = null);
        Task<List<RegionManagementNodeDto>> GetAllRegionData(bool isRealm = false, long? userId = null);
        Task<CreateRegionDto> CreateAsync(CreateRegionDto input);
        Task<DeleteResponseDto> DeleteAsync(string id, string type);
        Task<bool> SaveUserRealm(RegionHospitalMappingDto input);
        Task<CreateRegionDto> UpdateAsync(CreateRegionDto input);
        Task<IEnumerable<RegionManagementNodeDto>> GetAllRegionsMultiSelect(long? userId = null);
    }
}
