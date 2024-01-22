using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using CaseMix.Attributes;
using CaseMix.Controllers;
using CaseMix.Dto;
using CaseMix.Entities;
using CaseMix.Extensions;
using CaseMix.Services.BodyStructures;
using CaseMix.Services.BodyStructures.Dto;
using CaseMix.Services.PreOperativeAssessments;
using CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData;
using CaseMix.Services.PreOperativeAssessments.Dto.SetPoapData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SnomedApi;
using SnomedApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IList = System.Collections.IList;

namespace CaseMix.Web.Host.Controllers
{
    [Route("")]
    [ApiController]
    [ThirdPartyApi]
    [AbpAuthorize]
    public class CaseMixController : CaseMixControllerBase
    {
        private readonly SnomedApiConfiguration _snomedApiConfiguration;
        private readonly IRepository<BodyStructure, int> _bodyStructuresRepository;
        private readonly IRepository<PatientSurgeryProgress, int> _patientSurgeryProgressesRepository;
        private readonly IBodyStructuresAppService _bodyStructuresAppService;
        private readonly IPreOperativeAssessmentsAppService _preOperativeAssessmentsAppService;

        public CaseMixController(
            IOptions<SnomedApiConfiguration> snomedApiConfiguration,
            IRepository<BodyStructure, int> bodyStructuresRepository,
            IRepository<PatientSurgeryProgress, int> patientSurgeryProgressesRepository,
            IBodyStructuresAppService bodyStructuresAppService,
            IPreOperativeAssessmentsAppService preOperativeAssessmentsAppService
            )
        {
            _snomedApiConfiguration = snomedApiConfiguration.Value;
            _bodyStructuresRepository = bodyStructuresRepository;
            _patientSurgeryProgressesRepository = patientSurgeryProgressesRepository;
            _bodyStructuresAppService = bodyStructuresAppService;
            _preOperativeAssessmentsAppService = preOperativeAssessmentsAppService;
        }

        [HttpGet("/getBodyStructureList")]
        [ProducesResponseType(typeof(IEnumerable<BodyStructureOutputDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<BodyStructureOutputDto>>> GetBodyStructureList()
        {
            var bodyStructures = await _bodyStructuresRepository.GetAll().ToListAsync();
            var bodyStructureResults = ObjectMapper.Map<IEnumerable<BodyStructureOutputDto>>(bodyStructures);
            return Ok(bodyStructureResults);
        }

        [HttpGet("/getSurgicalProcedure")]
        [ProducesResponseType(typeof(IEnumerable<SurgicalProcedureOutputDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<SurgicalProcedureOutputDto>>> GetSurgicalProcedure([FromQuery][Required] string snomedId = "80891009")
        {
            using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
            {
                var result = await snomedApi.FindConceptsUsingGETMethodAsync(_snomedApiConfiguration.Branch, limit: 50, offset: 0, ecl: $"* : 260686004 |Method (attribute)| = 129304002 |Excision - action (qualifier value)| AND 405813007 |Procedure site - Direct (attribute)| = {snomedId} |(body structure)|");
                var surgicalProcedures = new List<SurgicalProcedureOutputDto>();
                if (result != null && result.Items != null)
                {
                    var items = (IList)result.Items;
                    foreach (var item in items)
                    {
                        var jObj = item as JObject;
                        var concept = jObj.ToObject<Concept>();
                        var surgicalProcedure = ObjectMapper.Map<SurgicalProcedureOutputDto>(concept);
                        surgicalProcedures.Add(surgicalProcedure);
                    }
                }
                return Ok(surgicalProcedures);
            }
        }

        [HttpGet("/getSnomedChildren")]
        [ProducesResponseType(typeof(IEnumerable<SnomedChildrenOutputDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<SnomedChildrenOutputDto>>> GetSnomedChildren([FromQuery][Required] int snomedId = 265466007)
        {
            using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
            {
                var conceptResult = await snomedApi.FindConceptsUsingGETMethodAsync(_snomedApiConfiguration.Branch, ecl: $"<! {snomedId}");
                var snomedChildren = new List<SnomedChildrenOutputDto>();
                if (conceptResult != null && conceptResult.Items != null)
                {

                    var items = (IList)conceptResult.Items;
                    foreach (var item in items)
                    {
                        var jObj = item as JObject;
                        var concept = jObj.ToObject<Concept>();

                        var browserConceptResult = await snomedApi.FindBrowserConceptUsingGETAsync(_snomedApiConfiguration.Branch, concept.ConceptId, _snomedApiConfiguration.Language);
                        var activeGroupedRelationships = browserConceptResult.Relationships.Where(e => e.Active.HasValue && e.Active.Value).GroupBy(e => e.GroupId).Select(e => new
                        {
                            GroupId = e.Key,
                            Relationships = e.Select(r => r),
                        }).OrderBy(e => e.GroupId);
                        foreach (var activeGroupedRelationship in activeGroupedRelationships)
                        {
                            var procedureSite = activeGroupedRelationship.Relationships.Where(e => e.Type.Pt.Term.ToLower().Contains(_snomedApiConfiguration.BrowserConceptProcedureTypeKey)).FirstOrDefault();
                            var method = activeGroupedRelationship.Relationships.Where(e => e.Type.Pt.Term.ToLower().Contains(_snomedApiConfiguration.BrowserConceptMethodKey)).FirstOrDefault();
                            if (procedureSite != null && method != null)
                            {
                                var snomedChild = new SnomedChildrenOutputDto()
                                {
                                    id = Convert.ToInt32(browserConceptResult.ConceptId),
                                    name = browserConceptResult.Fsn.Term,
                                    procedure_site = procedureSite.Target.Pt.Term,
                                    method = method.Target.Pt.Term,
                                };
                                snomedChildren.Add(snomedChild);
                            }
                        }
                    }
                }

                return Ok(snomedChildren);
            }
        }

        [HttpGet("/getProcedureCount")]
        [ProducesResponseType(typeof(IEnumerable<ProcedureCountOutputDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ProcedureCountOutputDto>>> GetProcedureCount([FromQuery][Required] int snomedId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = _patientSurgeryProgressesRepository.GetAll().Where(e => e.SnomedId == snomedId);
            var patientSurgeryProgress = await query.FirstOrDefaultAsync();
            if (patientSurgeryProgress != null)
            {
                int count = (await query
                    .ToListAsync())
                    .WhereIf(startDate.HasValue && endDate.HasValue, e => e.SurgeryDate.IsWithinDateRage(startDate.Value, endDate.Value))
                    .Count();
                var procedureCount = new ProcedureCountOutputDto()
                {
                    snomedid = patientSurgeryProgress.SnomedId,
                    snomed_desc = patientSurgeryProgress.SnomedDesc,
                    procedure_count = count,
                };
                return Ok(procedureCount);
            }

            return Ok();
        }

        [HttpGet("/getMenuItems/{bodyStructureId}")]
        [ProducesResponseType(typeof(IEnumerable<MenuItemOutputDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<ActionResult<List<MenuItemOutputDto>>> GetMenuItems(int bodyStructureId)
        {
            var results = await _bodyStructuresAppService.GetMenu(bodyStructureId);
            return Ok(results);
        }

        [HttpGet("/refreshMenuItems/{bodyStructureId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> RefreshMenuItems(int bodyStructureId)
        {
            await _bodyStructuresAppService.RefreshMenuCache(bodyStructureId);
            return Ok();
        }

        [HttpGet("/clearCache")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> ClearCache()
        {
            await _bodyStructuresAppService.ClearMenuCache();
            return Ok();
        }

        [HttpPost("/setPoapData")]
        [ProducesResponseType(typeof(SetPoapDataOutputDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult> SetPoapData(SetPoapDataInputDto input)
        {
            var output = await _preOperativeAssessmentsAppService.SetPoapData(input);
            if (output.Errors.Count > 0)
            {
                return BadRequest(output);
            }
            return Ok(output);
        }

        [HttpGet("/getPoapData")]
        [ProducesResponseType(typeof(IEnumerable<GetPoapDataOutputDto>), 200)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<ActionResult<List<MenuItemOutputDto>>> GetMenuItems([FromQuery]GetPoapDataInputDto input)
        {
            var output = await _preOperativeAssessmentsAppService.GetPoapData(input);
            return Ok(output);
        }
    }
}