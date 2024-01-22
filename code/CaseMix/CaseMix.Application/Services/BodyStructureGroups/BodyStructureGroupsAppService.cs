using Abp.Domain.Repositories;
using Abp.Extensions;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.BodyStructureProcedures.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SnomedApi;
using SnomedApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.BodyStructureGroups
{
    public class BodyStructureGroupsAppService : CaseMixAppServiceBase, IBodyStructureGroupsAppService
    {
        private readonly SnomedApiConfiguration _snomedApiConfiguration;
        private readonly IRepository<BodyStructureGroup, Guid> _bodyStructureGroupsRepository;
        private readonly IRepository<UserHospital, Guid> _userHospitalRepository;
        private readonly IRepository<BodyStructure, int> _bodyStructuresRepository;
        private readonly IRepository<SpecialtyInfo, int> _specialtyInfoRepository;

        public BodyStructureGroupsAppService(
            IOptions<SnomedApiConfiguration> snomedApiConfiguration,
            IRepository<BodyStructure, int> bodyStructuresRepository,
            IRepository<BodyStructureGroup, Guid> bodyStructureGroupsRepository,
            IRepository<UserHospital, Guid> userHospitalRepository,
            IRepository<SpecialtyInfo, int> specialtyInfoRepository)
        {
            _snomedApiConfiguration = snomedApiConfiguration.Value;
            _bodyStructureGroupsRepository = bodyStructureGroupsRepository;
            _userHospitalRepository = userHospitalRepository;
            _bodyStructuresRepository = bodyStructuresRepository;
            _specialtyInfoRepository = specialtyInfoRepository;
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetAll()
        {
            var groups = await _bodyStructureGroupsRepository.GetAll()
                .Include(e => e.BodyStructures)
                .Include(e => e.SurgeonSpecialties)
                    .ThenInclude(e => e.User)
                .OrderBy(e => e.DisplayOrder)
                .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e))
                .ToListAsync();
            foreach (var group in groups)
            {
                group.BodyStructures = group.BodyStructures.OrderBy(e => e.DisplayOrder);
            }

            return groups;
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetAllWithSubProcedures()
        {
            try
            {
                var groups = await _bodyStructureGroupsRepository.GetAll()
                .Include(e => e.BodyStructures)
                    .ThenInclude(e => e.BodyStructureQueries)
                .Include(e => e.BodyStructures)
                    .ThenInclude(e => e.BodyStructureSubProcedures)
                .Include(e => e.SurgeonSpecialties)
                    .ThenInclude(e => e.User)
                .OrderBy(e => e.DisplayOrder)
                .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e))
                .ToListAsync();

                foreach (var group in groups)
                {
                    if (group.BodyStructures != null)
                    {
                        foreach (var structure in group.BodyStructures)
                        {
                            if (structure.BodyStructureQueries != null)
                            {
                                if (structure.BodyStructureQueries.Count() > 0)
                                {
                                    try
                                    {
                                        using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
                                        {
                                            var conceptResult = await snomedApi.FindConceptsUsingGETMethodAsync(_snomedApiConfiguration.Branch, ecl: await GetSnomedQuery(structure.Id), limit: 1000);
                                            if (conceptResult != null && conceptResult.Items != null)
                                            {
                                                var conceptResultItems = (IList)conceptResult.Items;
                                                var exempted = string.Join(",", Enum.GetValues(typeof(ExemptedBodyStructures)).Cast<int>()).Split(",");
                                                var isExempted = exempted.Any(e => e == structure.Id.ToString());

                                                if (!isExempted)
                                                {
                                                    foreach (var conceptResultItem in conceptResultItems)
                                                    {
                                                        var jConceptResultItemObject = conceptResultItem as JObject;
                                                        var concept = jConceptResultItemObject.ToObject<Concept>();
                                                        //Id = concept.ConceptId
                                                        //Name = concept.Fsn.Term

                                                        BodyStructureSubProcedureDto _ = new BodyStructureSubProcedureDto();
                                                        _.BodyStructure = structure;
                                                        _.BodyStructureId = structure.Id;
                                                        _.SnomedId = concept.ConceptId;
                                                        _.Description = concept.Fsn.Term;

                                                        if (structure.BodyStructureSubProcedures == null)
                                                        {
                                                            IList<BodyStructureSubProcedureDto> bodyStructureSubProcedures = new List<BodyStructureSubProcedureDto>();
                                                            bodyStructureSubProcedures.Add(_);

                                                            structure.BodyStructureSubProcedures = bodyStructureSubProcedures;
                                                        }
                                                        else
                                                        {
                                                            structure.BodyStructureSubProcedures.Add(_);
                                                        }
                                                    }
                                                }
                                            }
                                        } //End of using SnowStorm
                                    }
                                    catch(Exception ex)
                                    {
                                        Logger.Error(ex.Message);
                                    }

                                }//End of Condition
                            }
                        }
                    }

                    group.BodyStructures = group.BodyStructures.OrderBy(e => e.DisplayOrder);
                }


                //var results = groups.Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e));
                //foreach (var group in results)
                //{
                //    group.BodyStructures = group.BodyStructures.OrderBy(e => e.DisplayOrder);
                //}

                //return results;

                return groups;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return new List<BodyStructureGroupDto>();
            }

        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetByHospital(string hospitalId)
        {
            var groups = await _bodyStructureGroupsRepository.GetAll()
            .Include(e => e.BodyStructures)
            .Include(e => e.HospitalSpecialties)
            .Where(e => e.HospitalSpecialties.Any(x => x.HospitalId == hospitalId))
            .OrderBy(e => e.DisplayOrder)
            .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e))
            .ToListAsync();
            foreach (var group in groups)
            {
                group.BodyStructures = group.BodyStructures.OrderBy(e => e.DisplayOrder);
            }

            return groups;
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetBySurgeon(long surgeonId)
        {
            var groups = await _bodyStructureGroupsRepository.GetAll()
            .Include(e => e.BodyStructures)
            .Include(e => e.SurgeonSpecialties)
            .Where(e => e.SurgeonSpecialties.Any(x => x.SurgeonId == surgeonId))
            .OrderBy(e => e.DisplayOrder)
            .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e))
            .ToListAsync();
            foreach (var group in groups)
            {
                group.BodyStructures = group.BodyStructures.OrderBy(e => e.DisplayOrder);
            }

            return groups;
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetByBodyStructureGroupId(Guid? bodyStructureGroupId)
        {
            var groups = await _bodyStructureGroupsRepository.GetAll()
            .Include(e => e.BodyStructures)
            .Where(e => e.Id == bodyStructureGroupId)
            .OrderBy(e => e.DisplayOrder)
            .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e))
            .ToListAsync();
            foreach (var group in groups)
            {
                group.BodyStructures = group.BodyStructures.OrderBy(e => e.DisplayOrder);
            }

            return groups;
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetAllBodyStructureGroupByUsers(string hospitalId)
        {
            var userIds = GetAllUserIdsByHospital(hospitalId);
            var groups = await _bodyStructureGroupsRepository.GetAll()
                .Include(e => e.SurgeonSpecialties)
                    .ThenInclude(e => e.User)
                .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e))
                .ToListAsync();

            foreach (var group in groups)
            {
                if (group.SurgeonSpecialties.Count() > 0)
                    group.SurgeonSpecialties = group.SurgeonSpecialties.Where(e => userIds.Any(c => c == e.SurgeonId)).ToList();
            }

            return groups;
        }

        public async Task<IEnumerable<SpecialtyInfoDto>> GetHospitalSpecialty(string hospitalId)
        {
            var query = await _specialtyInfoRepository.GetAll()
                .Where(_ => _.HospitalId == hospitalId)
                .Select(e => ObjectMapper.Map<SpecialtyInfoDto>(e))
                .ToListAsync();

            return query;
        }

        public async Task SaveHospitalSpecialties(IEnumerable<SpecialtyInfoDto> inputs)
        {
            await _specialtyInfoRepository.DeleteAsync(e => e.HospitalId == inputs.FirstOrDefault().HospitalId);
            inputs = inputs.Where(e => e.IsSelected);
            foreach (var input in inputs)
            {
                var specialty = ObjectMapper.Map<SpecialtyInfo>(input);
                specialty.CreatedBy = AbpSession.UserId.Value;
                specialty.CreatedDate = DateTime.UtcNow;

                await _specialtyInfoRepository.InsertAsync(specialty);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private IQueryable<long> GetAllUserIdsByHospital(string hospitalId)
        {
            return _userHospitalRepository.GetAll()
                .Where(e => e.HospitalId == hospitalId)
                .Select(e => e.UserId);
        }

        private async Task<string> GetSnomedQuery(int bodyStructureId)
        {
            var bodyStructure = await _bodyStructuresRepository.GetAll()
                .Where(e => e.Id == bodyStructureId)
                .Include(e => e.BodyStructureQueries)
                .FirstOrDefaultAsync();

            var queries = new List<string>();
            foreach (var query in bodyStructure.BodyStructureQueries.OrderBy(e => e.QueryOrder))
            {
                if (!query.QuerySimplified.IsNullOrWhiteSpace())
                {
                    queries.Add($"({query.QuerySimplified})");
                }
            }

            string sQuery = string.Join(" OR ", queries);
            return sQuery;
        }

    }
}
