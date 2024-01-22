using Abp.Domain.Repositories;
using Abp.Extensions;
using CaseMix.Configuration;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Services.BodyStructures.Dto;
using Enyim.Caching;
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

namespace CaseMix.Services.BodyStructures
{
    public class BodyStructuresAppService : CaseMixAppServiceBase, IBodyStructuresAppService
    {
        //@TODO: move configurations to ABP configuration module
        private readonly SnomedApiConfiguration _snomedApiConfiguration;
        private readonly MemcachedConfiguration _memcachedConfiguration;
        private readonly IRepository<BodyStructure> _bodyStructuresRepository;
        private readonly IMemcachedClient _memcachedClient;
        private readonly IRepository<BodyStructureSubProcedure, int> _bodyStructureSubProcedureRepository;

        public BodyStructuresAppService(
            IOptions<SnomedApiConfiguration> snomedApiConfiguration,
            IOptions<MemcachedConfiguration> memcachedConfiguration,
            IRepository<BodyStructure> bodyStructuresRepository,
            IMemcachedClient memcachedClient,
            IRepository<BodyStructureSubProcedure, int> bodyStructureSubProcedureRepository
            )
        {
            _snomedApiConfiguration = snomedApiConfiguration.Value;
            _memcachedConfiguration = memcachedConfiguration.Value;
            _bodyStructuresRepository = bodyStructuresRepository;
            _memcachedClient = memcachedClient;
            _bodyStructureSubProcedureRepository = bodyStructureSubProcedureRepository;
        }

        public async Task<IEnumerable<BodyStructureDto>> GetAll()
        {
            var bodyStructures = await _bodyStructuresRepository.GetAll()
                .Select(e => ObjectMapper.Map<BodyStructureDto>(e))
                .ToListAsync();
            return bodyStructures;
        }

        public IEnumerable<ProcedureMethodDto> GetMethods()
        {
            return new List<ProcedureMethodDto>() {
                new ProcedureMethodDto()
                {
                    Id = "excision-272212",
                    Name = "Excision, 272212",
                }, 
            };
        }

        public async Task<IEnumerable<MenuItemOutputDto>> GetMenu(int id)
        {
            string cacheKey = $"{_memcachedConfiguration.MenuCacheKey}-{id}-steps";
            List<MenuItemOutputDto> results = await _memcachedClient.GetValueOrCreateAsync(
                cacheKey,
                _memcachedConfiguration.CacheLifetimeSeconds,
                async () => await GetSurgicalProcedureMenuItems(id)
            );
            return results;
        }

        public async Task RefreshMenuCache(int id)
        {
            string cacheKey = $"{_memcachedConfiguration.MenuCacheKey}-{id}-steps";
            var results = await GetSurgicalProcedureMenuItems(id);
            await _memcachedClient.SetAsync(
                cacheKey,
                results,
                _memcachedConfiguration.CacheLifetimeSeconds
            );
        }

        public async Task ClearMenuCache()
        {
            await _memcachedClient.FlushAllAsync();
        }

        //private async Task<List<MenuItemOutputDto>> GetBodyStructureMenuItems(int id)
        //{
        //    var bodyStructures = await _bodyStructuresRepository.GetAll()
        //        .Where(e => e.Id == id)
        //        .ToListAsync();
        //    var bodyStructureItems = new List<MenuItemOutputDto>();
        //    if (bodyStructures != null && bodyStructures.Any())
        //    {
        //        using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
        //        {
        //            foreach (var bodyStructure in bodyStructures)
        //            {
        //                var bodyStructureItem = new MenuItemOutputDto()
        //                {
        //                    Id = bodyStructure.Id.ToString(),
        //                    Name = bodyStructure.Description,
        //                    Children = await GetSurgicalProcedureMenuItems(bodyStructure.Id),
        //                };
        //                bodyStructureItems.Add(bodyStructureItem);
        //            }
        //        }
        //    }
        //    return bodyStructureItems;
        //}

        private async Task<List<MenuItemOutputDto>> GetSurgicalProcedureMenuItems(int id)
        {
            using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
            {
                var surgicalProcedureItems = new List<MenuItemOutputDto>();
                var conceptResult = await snomedApi.FindConceptsUsingGETMethodAsync(_snomedApiConfiguration.Branch, ecl: await GetSnomedQuery(id), limit: 1000);
                if (conceptResult != null && conceptResult.Items != null)
                {
                    var conceptResultItems = (IList)conceptResult.Items;
                    var exempted = string.Join(",", Enum.GetValues(typeof(ExemptedBodyStructures)).Cast<int>()).Split(",");
                    var isExempted = exempted.Any(e => e == id.ToString());

                    if (!isExempted)
                    {
                        foreach (var conceptResultItem in conceptResultItems)
                        {
                            var jConceptResultItemObject = conceptResultItem as JObject;
                            var concept = jConceptResultItemObject.ToObject<Concept>();
                            var surgicalProcedureItem = new MenuItemOutputDto()
                            {
                                Id = concept.ConceptId,
                                Name = concept.Fsn.Term,
                                ShowButtonDevProc = false //default to false if coming from Snomed API site
                                //Children = await GetSnomedChildrenMenuItems(concept.ConceptId),
                            };

                            surgicalProcedureItems.Add(surgicalProcedureItem);
                        }
                    }

                    var subprocedures = await _bodyStructureSubProcedureRepository.GetAll()
                                        .Where(e => e.BodyStructureId == id)
                                        .ToListAsync();

                    if(subprocedures.Count > 0)
                    {
                        foreach (var subprocedureItem in subprocedures)
                        {
                            var item = new MenuItemOutputDto()
                            {
                                Id = subprocedureItem.SnomedId,
                                Name = subprocedureItem.Description,
                                ShowButtonDevProc = subprocedureItem.ShowButtonDevProc                                
                            };
                            surgicalProcedureItems.Add(item);
                        }
                    }
                }
                return surgicalProcedureItems;
            }
        }

        //private async Task<List<MenuItemOutputDto>> GetSnomedChildrenMenuItems(string surgicalProcedureId)
        //{
        //    using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
        //    {
        //        var snomedChildrenItems = new List<MenuItemOutputDto>();
        //        var conceptResult = await snomedApi.FindConceptsUsingGETMethodAsync(_snomedApiConfiguration.Branch, ecl: $"<! {surgicalProcedureId}");
        //        if (conceptResult != null && conceptResult.Items != null)
        //        {

        //            var items = (IList)conceptResult.Items;
        //            foreach (var item in items)
        //            {
        //                var jObj = item as JObject;
        //                var concept = jObj.ToObject<Concept>();

        //                var browserConceptResult = await snomedApi.FindBrowserConceptUsingGETAsync(_snomedApiConfiguration.Branch, concept.ConceptId, _snomedApiConfiguration.Language);
        //                var snomedChildrenItem = new MenuItemOutputDto()
        //                {
        //                    Id = browserConceptResult.ConceptId,
        //                    Name = browserConceptResult.Fsn.Term,
        //                    Children = new List<MenuItemOutputDto>(),
        //                };

        //                var activeGroupedRelationships = browserConceptResult.Relationships.Where(e => e.Active.HasValue && e.Active.Value).GroupBy(e => e.GroupId).Select(e => new
        //                {
        //                    GroupId = e.Key,
        //                    Relationships = e.Select(r => r),
        //                }).OrderBy(e => e.GroupId);
        //                foreach (var activeGroupedRelationship in activeGroupedRelationships)
        //                {
        //                    var procedureSite = activeGroupedRelationship.Relationships.Where(e => e.Type.Pt.Term.ToLower().Contains(_snomedApiConfiguration.BrowserConceptProcedureTypeKey)).FirstOrDefault();
        //                    var method = activeGroupedRelationship.Relationships.Where(e => e.Type.Pt.Term.ToLower().Contains(_snomedApiConfiguration.BrowserConceptMethodKey)).FirstOrDefault();
        //                    if (procedureSite != null && method != null)
        //                    {
        //                        snomedChildrenItem.Children.Add(new MenuItemOutputDto()
        //                        {
        //                            Id = browserConceptResult.ConceptId,
        //                            Name = $"{procedureSite.Target.Pt.Term} ({method.Target.Pt.Term})",
        //                        });
        //                    }
        //                }
        //                snomedChildrenItems.Add(snomedChildrenItem);
        //            }
        //        }
        //        return snomedChildrenItems;
        //    }
        //}

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
