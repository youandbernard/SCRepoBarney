using Abp.Application.Services;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.Manufactures.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.Device
{
    public interface IDeviceAppService : IAsyncCrudAppService<DeviceDto, int, PagedDeviceResultRequestDto>
    {
        Task<bool> EnableDisableDevice(int deviceId, int stat);
        Task<bool> EnableDisableSelected(List<int> deviceIds, int stat);
        Task<bool> DevicesAvailability(List<int> deviceIds, int stat, string hospitalId);
        Task<DeviceDto> GetByDeviceId(int id);
        Task<IEnumerable<DeviceClassDto>> GetAllDeviceClass();
        Task<IEnumerable<DevicesTermCodeDto>> GetByDeviceGMDNCode(string gmdntermCode);
        Task<IEnumerable<DevicesTermCodeDto>> GetByDeviceCode(string gmdntermCode);
        Task<IEnumerable<BodyStructureGroupDto>> GetBodyStructureGroups();
        Task<IEnumerable<ManufactureDto>> GetManufacturers();
        Task<IEnumerable<DeviceFamilyDto>> GetAllDeviceFamily(Guid bodyStructureGroupId);
    }
}
