using Abp.Application.Services;
using CaseMix.Services.BodyStructureGroups.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.BodyStructureGroups
{
    public interface IBodyStructureGroupsAppService : IApplicationService
    {
        Task<IEnumerable<BodyStructureGroupDto>> GetAll();
        Task<IEnumerable<BodyStructureGroupDto>> GetAllWithSubProcedures();
        Task<IEnumerable<BodyStructureGroupDto>> GetByHospital(string hospitalId);
        Task<IEnumerable<BodyStructureGroupDto>> GetAllBodyStructureGroupByUsers(string hospitalId);
        Task<IEnumerable<BodyStructureGroupDto>> GetByBodyStructureGroupId(Guid? bodyStructureGroupId);
        Task<IEnumerable<SpecialtyInfoDto>> GetHospitalSpecialty(string hospitalId);
        Task SaveHospitalSpecialties(IEnumerable<SpecialtyInfoDto> inputs);
    }
}
