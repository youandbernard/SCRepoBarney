using Abp.Application.Services;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.PoapProcedureDevices.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.PoapProcedureDevices
{
    public interface IPoapProcedureDevicesAppService: IApplicationService
    {
        Task<IEnumerable<PoapProcedureDevicesDto>> GetByProcedureId(string poapProcedureId, string poapId);
        Task SavePoapProcedureDevices(IEnumerable<DeviceDto> deviceDtos, Guid poapId, Guid poapProcedureId, string snomedId);
    }
}
