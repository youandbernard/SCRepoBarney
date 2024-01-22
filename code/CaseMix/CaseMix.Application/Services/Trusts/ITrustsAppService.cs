using Abp.Application.Services;
using CaseMix.Services.Trusts.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CaseMix.Services.Trusts
{
    public interface ITrustsAppService : IApplicationService
    {
        Task<IEnumerable<TrustsDto>> GetAll(Guid? RegionId = null, string regionName = null, bool fromHospitalManagement = false, string countryName = "");
    }
}
