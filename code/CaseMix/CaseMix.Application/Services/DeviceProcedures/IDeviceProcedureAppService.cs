using Abp.Application.Services;
using CaseMix.Core.Shared.Models;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.DeviceProcedures.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.DeviceProcedures
{
    public interface IDeviceProcedureAppService : IApplicationService
    {
        Task<IEnumerable<BodyStructureGroupDto>> GetByDeviceId(int deviceId, string hospitalId);
        Task SaveSelectedDeviceProcedures(int deviceId, IEnumerable<TreeNodeInput> treeNodes);
        Task<IEnumerable<DeviceDto>> GetBySnomedId(string id, int bodyStructureId, string hospitalId, string brandName, int deviceFamilyId, string model);
        Task<DeviceBrandFamilyViewDto> GetBySnomedIdGrouped(string id, int bodyStructureId, string hospitalId, string bodyStructureGroupId, bool isFilterLicensedStatus);
    }
}
