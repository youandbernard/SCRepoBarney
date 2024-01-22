using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using CaseMix.DomainServices.Timezone;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Extensions;
using CaseMix.Services.PoapInstrumentPacks.Dto;
using CaseMix.Services.PoapProcedureDevices;
using CaseMix.Services.PoapProcedureDevices.Dto;
using CaseMix.Services.PoapRiskFactors.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData;
using CaseMix.Services.PreOperativeAssessments.Dto.SetPoapData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.PreOperativeAssessments
{
    public class PreOperativeAssessmentsAppService : AsyncCrudAppService<PreOperativeAssessment, PreOperativeAssessmentDto, Guid, PagedPoapResultRequestDto>, IPreOperativeAssessmentsAppService
    {
        private readonly IRepository<PoapProcedure, Guid> _poapProceduresRepository;
        private readonly IRepository<PoapRisk, Guid> _poapRisksRepository;
        private readonly IRepository<Patient, string> _patientsRepository;
        private readonly IRepository<HospitalPatient, Guid> _hospitalPatientsRepository;
        private readonly IRepository<PatientSurvey, Guid> _patientSurveysRepository;
        private readonly IRepository<ProcedureMethodType, Guid> _procedureMethodTypeRepository;
        private readonly IRepository<PoapProcedureMethodType, int> _poapProcedureMethodTypeRepository;
        private readonly IRepository<PoapRiskFactor, int> _poapRiskFactorRepository;
        private readonly IRepository<RiskMappingSetting, int> _riskMappingRepository;
        private readonly IRepository<CaseMix.Entities.PoapProcedureDevices, int> _devicePoapProcDevRepository;
        private readonly IRepository<PoapInstrumentPack, int> _poapInstrumentPackRepository;
        private readonly IRepository<BodyStructureSubProcedure, int> _bodyStructureSubProcedureRepository;
        private readonly IRepository<InstrumentPack, int> _instrumentPackRepository;
        private readonly ITimezoneDomainService _timezoneDomainService;

        public PreOperativeAssessmentsAppService(
            IRepository<CaseMix.Entities.PoapProcedureDevices, int> devicePoapProcDevRepository,
            IRepository<PreOperativeAssessment, Guid> repository,
            IRepository<PoapProcedure, Guid> poapProceduresRepository,
            IRepository<PoapRisk, Guid> poapRisksRepository,
            IRepository<Patient, string> patientsRepository,
            IRepository<HospitalPatient, Guid> hospitalPatientsRepository,
            IRepository<PatientSurvey, Guid> patientSurveysRepository,
            ITimezoneDomainService timezoneDomainService,
            IRepository<ProcedureMethodType, Guid> procedureMethodTypeRepository,
            IRepository<PoapProcedureMethodType, int> poapProcedureMethodTypeRepository,
            IRepository<BodyStructureSubProcedure, int> bodyStructureSubProcedureRepository,
            IRepository<PoapRiskFactor, int> poapRiskFactorRepository,
            IRepository<RiskMappingSetting, int> riskMappingSettingRepository,
            IRepository<PoapInstrumentPack, int> poapInstrumentPackRepository,
            IRepository<InstrumentPack, int> instrumentPackRepository
            ) : base(repository)
        {
            _poapProceduresRepository = poapProceduresRepository;
            _poapRisksRepository = poapRisksRepository;
            _patientsRepository = patientsRepository;
            _hospitalPatientsRepository = hospitalPatientsRepository;
            _patientSurveysRepository = patientSurveysRepository;
            _timezoneDomainService = timezoneDomainService;
            _procedureMethodTypeRepository = procedureMethodTypeRepository;
            _poapProcedureMethodTypeRepository = poapProcedureMethodTypeRepository;
            _poapRiskFactorRepository = poapRiskFactorRepository;
            _riskMappingRepository = riskMappingSettingRepository;
            _devicePoapProcDevRepository = devicePoapProcDevRepository;
            _poapInstrumentPackRepository = poapInstrumentPackRepository;
            _instrumentPackRepository = instrumentPackRepository;
            _bodyStructureSubProcedureRepository = bodyStructureSubProcedureRepository;
        }

        protected override IQueryable<PreOperativeAssessment> CreateFilteredQuery(PagedPoapResultRequestDto input)
        {
            try
            {
                IQueryable<PreOperativeAssessment> result;

                if (!input.isDisplayRiskAwaitingCompletion)
                {
                    result = base.CreateFilteredQuery(input)
                    .Where(e => e.HospitalId == input.HospitalId && !e.IsArchived && e.AwaitingRiskCompletion == (int)PoapAwaitingRiskCompletionStatus.Open && (e.Procedures.Where(w => w.IsPatientPreparation).Sum(s => s.ActualTime) == 0))
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), e => e.PatientId.ToLower().Contains(input.Keyword.ToLower())
                        || e.SurgeonName.ToLower().Contains(input.Keyword.ToLower())
                        || e.AnesthetistName.ToLower().Contains(input.Keyword.ToLower())
                        || e.BodyStructure.BodyStructureGroup.Name.ToLower().Contains(input.Keyword.ToLower())
                    )
                    .Include(e => e.BodyStructure)
                        .ThenInclude(e => e.BodyStructureGroup)
                    .Include(e => e.Procedures)
                        .ThenInclude(e => e.PoapProcedureDevices)
                            .ThenInclude(e => e.Device).AsNoTracking()
                    .Include(e => e.Theater);
                }
                else
                {
                    result = base.CreateFilteredQuery(input)
                    .Where(e => e.HospitalId == input.HospitalId && ((e.IsArchived && e.AwaitingRiskCompletion == (int)PoapAwaitingRiskCompletionStatus.True) || 
                        (e.AwaitingRiskCompletion != (int)PoapAwaitingRiskCompletionStatus.False && e.Procedures.Where(w => w.IsPatientPreparation).Sum(s => s.ActualTime) > 0)))
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), e => e.PatientId.ToLower().Contains(input.Keyword.ToLower())
                        || e.SurgeonName.ToLower().Contains(input.Keyword.ToLower())
                        || e.AnesthetistName.ToLower().Contains(input.Keyword.ToLower())
                        || e.BodyStructure.BodyStructureGroup.Name.ToLower().Contains(input.Keyword.ToLower())
                    )
                    .Include(e => e.BodyStructure)
                        .ThenInclude(e => e.BodyStructureGroup)
                    .Include(e => e.Procedures)
                        .ThenInclude(e => e.PoapProcedureDevices)
                            .ThenInclude(e => e.Device).AsNoTracking()
                    .Include(e => e.Theater);
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override async Task<PagedResultDto<PreOperativeAssessmentDto>> GetAllAsync(PagedPoapResultRequestDto input)
        {
            var output = await base.GetAllAsync(input);
            foreach (var poap in output.Items)
            {
                poap.TotalMeanTime = poap.Procedures.Sum(e => e.MeanTime);
            }

            return output;
        }

        public override async Task<PreOperativeAssessmentDto> GetAsync(EntityDto<Guid> input)
        {
            var output = await Repository.GetAll()
                .Include(e => e.Risks)
                .Include(e => e.Procedures)
                .Include(e => e.Theater)
                .Where(e => e.Id == input.Id)
                .Select(e => ObjectMapper.Map<PreOperativeAssessmentDto>(e))
                .FirstOrDefaultAsync();

            return output;
        }

        public async Task<PreOperativeAssessmentDto> GetPoapAsync(EntityDto<Guid> input, string timezone)
        {
            var output = await Repository.GetAll()
                .Include(e => e.Risks)
                .Include(e => e.Procedures)
                    .ThenInclude(e => e.PoapProcedureDevices)
                        .ThenInclude(e => e.Device)
                            .ThenInclude(e => e.Manufacturer).AsNoTracking()                            
                .Include(e => e.Theater)
                .Include(e => e.DiagnosticReport)
                .Where(e => e.Id == input.Id)
                .Select(e => ObjectMapper.Map<PreOperativeAssessmentDto>(e))
                .FirstOrDefaultAsync();

            output.SurgeryDate = _timezoneDomainService.ConvertToLocal(output.SurgeryDate, timezone);
            output.AssessmentDate = _timezoneDomainService.ConvertToLocal(output.AssessmentDate, timezone);
            output.ProcedureMethodTypes = await GetPoapMethodTypes(output.Id);
            output.InstrumentPacks = await GetPoapInstructionPacks(output.Id);

            if (output.Procedures != null)
            {
                foreach (var proc in output.Procedures)
                {
                    var isShows = _bodyStructureSubProcedureRepository.GetAll()
                        .Where(_ => _.SnomedId == proc.SnomedId && _.BodyStructureId == output.BodyStructureId).ToList();

                    var isShow = isShows.Where(_ => _.ShowButtonDevProc == true && _.BodyStructureId == output.BodyStructureId).FirstOrDefault();
                    proc.ShowButtonDevProc = isShow != null ? isShow.ShowButtonDevProc : false;
                }
            }

            return output;
        }

        [UnitOfWork]
        public override async Task<PreOperativeAssessmentDto> CreateAsync(PreOperativeAssessmentDto input)
        {
            int displayOrder = 0;
            foreach (var procedure in input.Procedures)
            {
                procedure.DisplayOrder = displayOrder;
                displayOrder++;
            }

            var patientCount = await _patientsRepository.CountAsync(e => e.Id == input.PatientId);
            if (patientCount == 0)
            {
                var patient = new Patient
                {
                    Id = input.PatientId,
                    Gender = input.Gender,
                    DateOfBirth = new DateTime(input.DateOfBirthYear.Value, 1, 1)
                };
                await _patientsRepository.InsertAsync(patient);

                var hospitalPatient = new HospitalPatient
                {
                    PatientId = patient.Id,
                    HospitalId = input.HospitalId
                };
                await _hospitalPatientsRepository.InsertAsync(hospitalPatient);
            }

            var surgeryDate = _timezoneDomainService.ConvertToUtc(input.SurgeryDate, input.Timezone);
            var assessmentDate = _timezoneDomainService.ConvertToUtc(input.AssessmentDate, input.Timezone);
            //set awaiting risk completion status to open
            input.AwaitingRiskCompletion = (int)PoapAwaitingRiskCompletionStatus.Open;

            input.SurgeryDate = surgeryDate;
            input.AssessmentDate = assessmentDate;
            var result = new PreOperativeAssessmentDto();

            try
            {
                List<PoapProcedureDevicesDto> poapProcedureDevicesDtos = new List<PoapProcedureDevicesDto>();
                foreach (var proc in input.Procedures)
                {
                    if (proc.PoapProcedureDevices != null)
                    {
                        foreach (var procd in proc.PoapProcedureDevices)
                        {
                            poapProcedureDevicesDtos.Add(procd);
                        }

                        proc.PoapProcedureDevices = null;
                    }
                }

                result = await base.CreateAsync(input);
                await CreateSurveyFromPoapAsync(result);
                await SavePoapProcedureMethodTypes(result.Id, input.ProcedureMethodTypes);
                await SavePoapProcedureDevice(result.Id, result.Procedures, poapProcedureDevicesDtos);
                await SavePoapInstrumentPacks(result.Id, input.InstrumentPacks);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        [UnitOfWork]
        public override async Task<PreOperativeAssessmentDto> UpdateAsync(PreOperativeAssessmentDto input)
        {
            var poap = await base.Repository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.Id == input.Id);
            input.Theater = null;
            int displayOrder = 0;
            foreach (var procedure in input.Procedures)
            {
                procedure.DisplayOrder = displayOrder;
                displayOrder++;
            }
            await _poapProceduresRepository.DeleteAsync(e => e.PreOperativeAssessmentId == input.Id);
            await _poapRisksRepository.DeleteAsync(e => e.PreOperativeAssessmentId == input.Id);

            var patientCount = await _patientsRepository.CountAsync(e => e.Id == input.PatientId);
            if (patientCount == 0)
            {
                var patient = new Patient
                {
                    Id = input.PatientId,
                    Gender = input.Gender,
                    DateOfBirth = new DateTime(input.DateOfBirthYear.Value, 1, 1)
                };
                await _patientsRepository.InsertAsync(patient);

                var hospitalPatient = new HospitalPatient
                {
                    PatientId = patient.Id,
                    HospitalId = input.HospitalId
                };
                await _hospitalPatientsRepository.InsertAsync(hospitalPatient);
            }

            if (poap.SurgeryDate != input.SurgeryDate)
            {
                var surgeryDate = _timezoneDomainService.ConvertToUtc(input.SurgeryDate, input.Timezone);
                var assessmentDate = _timezoneDomainService.ConvertToUtc(input.AssessmentDate, input.Timezone);
                input.SurgeryDate = surgeryDate;
                input.AssessmentDate = assessmentDate;
            }

            List<PoapProcedureDevicesDto> poapProcedureDevicesDtos = new List<PoapProcedureDevicesDto>();
            foreach (var proc in input.Procedures)
            {
                if (proc.PoapProcedureDevices != null)
                {
                    foreach (var procd in proc.PoapProcedureDevices)
                    {
                        poapProcedureDevicesDtos.Add(procd);
                    }

                    proc.PoapProcedureDevices = null;
                }
            }

            var result = await base.UpdateAsync(input);
            await SavePoapProcedureMethodTypes(result.Id, input.ProcedureMethodTypes);
            await UpdateDiagnosticId(result.Id, input.DiagnosticReportId);
            await SavePoapProcedureDevice(result.Id, input.Procedures, poapProcedureDevicesDtos);
            await SavePoapInstrumentPacks(result.Id, input.InstrumentPacks);

            return result;
        }

        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            string patientId = string.Empty;
            var poap = Repository.GetAsync(input.Id).Result;
            if (poap != null)
            {
                patientId = poap.PatientId;
            }

            await _poapProceduresRepository.DeleteAsync(e => e.PreOperativeAssessmentId == input.Id);
            await _poapRisksRepository.DeleteAsync(e => e.PreOperativeAssessmentId == input.Id);
            await _patientSurveysRepository.DeleteAsync(e => e.PreOperativeAssessmentId == input.Id);

            if (!string.IsNullOrWhiteSpace(patientId))
                await _hospitalPatientsRepository.DeleteAsync(e => e.PatientId == patientId);

            await base.DeleteAsync(input);
        }

        public async Task SavePoapRiskFactors(PreOperativeAssessmentDto preOperativeAssessment)
        {
            try
            {
                await _poapRiskFactorRepository.DeleteAsync(e => e.PreoperativeAssessmentId == preOperativeAssessment.Id);

                var riskFactors = preOperativeAssessment.Risks.Select(e => new PoapRiskFactorDto
                {
                    PreoperativeAssessmentId = preOperativeAssessment.Id,
                    DiagnosticReportId = (int)preOperativeAssessment.DiagnosticReportId,
                    RiskId = (Guid)e.Id
                }).ToList();

                foreach (var riskFactor in riskFactors)
                {
                    var data = new PoapRiskFactor
                    {
                        PreoperativeAssessmentId = riskFactor.PreoperativeAssessmentId,
                        RiskId = riskFactor.RiskId,
                        DiagnosticReportId = riskFactor.DiagnosticReportId
                    };

                    await _poapRiskFactorRepository.InsertAsync(data);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task SaveUnselectedRiskFactors(Guid id, IEnumerable<DiagnosticRiskFactorsMappingDto> riskFactorMappings)
        {
            try
            {
                var unselectedPoapRisks = new List<PoapRisk>();
                //add the unselected risk factors
                foreach (var riskFactorMapping in riskFactorMappings)
                {
                    var data = new PoapRisk()
                    {
                        Group = riskFactorMapping.Group,
                        Key = riskFactorMapping.SnomedId,
                        Value = "true",
                        MeanTime = 0,
                        StandardDeviation = 0,
                        PreOperativeAssessmentId = id,
                        DiagnosticId = riskFactorMapping.DiagnosticId
                    };
                    var response = await _poapRisksRepository.InsertAsync(data);
                    unselectedPoapRisks.Add(response);
                }

                foreach (var poapRisk in unselectedPoapRisks)
                {
                    var data = new PoapRiskFactor
                    {
                        PreoperativeAssessmentId = poapRisk.PreOperativeAssessmentId,
                        RiskId = poapRisk.Id,
                        DiagnosticReportId = (int)poapRisk.DiagnosticId
                    };

                    await _poapRiskFactorRepository.InsertAsync(data);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<SetPoapDataOutputDto> SetPoapData(SetPoapDataInputDto input)
        {
            var output = new SetPoapDataOutputDto
            {
                PoapRecordsReceived = input.Timings.Count()
            };

            foreach (var timing in input.Timings)
            {
                var isPoapExists = await Repository.GetAll().AnyAsync(e => e.Id == timing.PoapId);
                if (isPoapExists)
                {
                    foreach (var subProcedure in timing.SubProcedures)
                    {
                        var procedure = await _poapProceduresRepository.GetAll()
                            .FirstOrDefaultAsync(e => e.PreOperativeAssessmentId == timing.PoapId && e.Id == subProcedure.SubProcedureId);
                        if (procedure != null)
                        {
                            procedure.ActualTime = subProcedure.ActualTime;
                        }
                    }
                }
                else
                {
                    output.PoapRecordsFailed++;
                    output.Errors.Add(new SetPoapDataErrorOutputDto()
                    {
                        PoapId = timing.PoapId,
                        Error = "poapId not found",
                    }); ;
                }
            }

            return output;
        }

        public async Task<GetPoapDataOutputDto> GetPoapData(GetPoapDataInputDto input)
        {
            var poaps = await Repository.GetAll()
                .Where(e => e.SurgeonId == input.SurgeonId)
                .WhereIf(input.StartDate.HasValue && input.EndDate.HasValue, e => e.SurgeryDate.Date >= input.StartDate.Value.Date && e.SurgeryDate.Date <= input.EndDate.Value.Date)
                .Include(e => e.Surgeon)
                .Include(e => e.Risks)
                .Include(e => e.Procedures)
                .Include(e => e.Ethnicity)
                .Include(e => e.Theater)
                .ToListAsync();

            var output = new GetPoapDataOutputDto()
            {
                SurgeonId = input.SurgeonId,
            };
            if (poaps != null && poaps.Any())
            {
                output.SurgeonId = poaps[0].SurgeonId;
                output.SurgeonName = poaps[0].SurgeonName;
                output.Poap = poaps.Select(p => new GetPoapDataPoapDto()
                {
                    PoapId = p.Id,
                    HospitalId = p.HospitalId,
                    TheatreId = p.Theater.TheaterId,
                    PatientId = p.PatientId,
                    PatientDOBYear = p.DateOfBirthYear ?? 0,
                    AssessmentDate = p.AssessmentDate,
                    SurgeryDate = p.SurgeryDate,
                    Gender = p.Gender.ToDescription(),
                    AnesthetistName = p.AnesthetistName,
                    TotalMeanTime = p.Procedures.Sum(e => e.MeanTime),
                    TotalStandardDeviation = p.Procedures.Sum(e => e.StandardDeviation),
                    Ethnicity = p?.Ethnicity?.Description ?? string.Empty,
                    Risks = p.Risks.Select(r => new GetPoapDataRiskDto()
                    {
                        RiskGroup = r.Group,
                        RiskKey = r.Key,
                        RiskValue = r.Value,
                        MeanTime = r.MeanTime,
                        StandardDeviation = r.StandardDeviation,
                    }),
                    SubProcedures = p.Procedures.Select(s => new GetPoapDataProcedureDto()
                    {
                        Name = s.Name,
                        OrderId = s.DisplayOrder,
                        ProcedureId = s.Id,
                        SnomedId = s.SnomedId,
                        ProcedureSite = s.ProcedureSite,
                        Method = s.Method,
                        MeanTime = s.MeanTime,
                        StandardDeviation = s.StandardDeviation,
                        IsRisk = s.IsRisk,
                    }),
                });
            }

            return output;
        }

        public async Task<IEnumerable<PreOperativeAssessmentDto>> GetPoapDataByTheaterAsync(Guid TheaterId)
        {
            var poaps = await Repository.GetAll()
                .Where(e => e.TheaterId == TheaterId)
                .Select(e => ObjectMapper.Map<PreOperativeAssessmentDto>(e))
                .ToListAsync();

            return poaps;
        }

        public async Task<IEnumerable<ProcedureMethodTypeDto>> GetAllProcedureMethodTypes()
        {
            var methodTypes = await _procedureMethodTypeRepository.GetAll()
                .Select(e => ObjectMapper.Map<ProcedureMethodTypeDto>(e))
                .ToListAsync();

            return methodTypes;
        }

        public async Task<IEnumerable<InstrumentPackDto>> GetAllInstrumentPacks()
        {
            var instrumentPacks = await _instrumentPackRepository.GetAll()
                .Select(e => ObjectMapper.Map<InstrumentPackDto>(e))
                .ToListAsync();

            return instrumentPacks;
        }

        public async Task UpdateAwaitingRiskCompletion(Guid id, int awaitingRiskCompletionStatus)
        {
            var poap = await Repository.GetAll()
                .Where(e => e.Id == id)
                .Select(e => ObjectMapper.Map<PreOperativeAssessmentDto>(e))
                .FirstOrDefaultAsync();

            if (poap == null)
                throw new Exception("Poap not found!");

            poap.AwaitingRiskCompletion = awaitingRiskCompletionStatus;


            var result = await base.UpdateAsync(poap);
        }

        private async Task<IEnumerable<ProcedureMethodTypeDto>> GetPoapMethodTypes(Guid preOperativeassessmentId)
        {
            var poapProcedureMethodTypeIds = await _poapProcedureMethodTypeRepository.GetAll()
                .Where(e => e.PreOperativeAssessmentId == preOperativeassessmentId)
                .Select(e => e.ProcedureTypeId)
                .ToListAsync();

            var methodTypes = await GetAllProcedureMethodTypes();
            var result = methodTypes.Where(e => poapProcedureMethodTypeIds.Any(c => e.Id == c)).ToList();

            return result;
        }

        private async Task<IEnumerable<InstrumentPackDto>> GetPoapInstructionPacks(Guid preOperativeassessmentId)
        {
            var poapInstrumentPackIds = await _poapInstrumentPackRepository.GetAll()
                .Where(e => e.PoapId == preOperativeassessmentId)
                .Select(e => e.InstrumentPackId)
                .ToListAsync();

            var instrumentPacks = await GetAllInstrumentPacks();
            var result = instrumentPacks.Where(e => poapInstrumentPackIds.Any(c => e.InstrumentPackId == c)).ToList();

            return result;
        }

        private async Task SavePoapProcedureMethodTypes(Guid preOperativeAssessmentId, IEnumerable<ProcedureMethodTypeDto> input)
        {
            await _poapProcedureMethodTypeRepository.DeleteAsync(e => e.PreOperativeAssessmentId == preOperativeAssessmentId);

            foreach (var item in input)
            {
                var data = new PoapProcedureMethodType
                {
                    PreOperativeAssessmentId = preOperativeAssessmentId,
                    ProcedureTypeId = item.Id
                };
                await _poapProcedureMethodTypeRepository.InsertAsync(data);
            }
        }

        private async Task SavePoapInstrumentPacks(Guid preOperativeAssessmentId, IEnumerable<InstrumentPackDto> input)
        {
            await _poapInstrumentPackRepository.DeleteAsync(e => e.PoapId == preOperativeAssessmentId);

            var currDt = DateTime.UtcNow;
            foreach (var item in input)
            {
                var data = new PoapInstrumentPack
                {
                    PoapId = preOperativeAssessmentId,
                    InstrumentPackId = item.InstrumentPackId,
                    InstrumentId = 1, //todo: fixed value for now
                    UserCode = string.Empty, //todo: fixed value for now
                    Quantity = 1, //todo: fixed value for now
                    EmbodiedCarbon = item.EmbodiedCarbon.Value,
                    CreatedBy = AbpSession.UserId.Value,
                    CreatedDate = currDt,
                    UpdatedBy = AbpSession.UserId.Value,
                    UpdatedDate = currDt,

                };
                await _poapInstrumentPackRepository.InsertAsync(data);
            }
        }

        private async Task CreateSurveyFromPoapAsync(PreOperativeAssessmentDto input)
        {
            var survey = new PatientSurvey()
            {
                PatientId = input.PatientId,
                PatientDobYear = input.DateOfBirthYear,
                SurgeryDate = input.SurgeryDate,
                SurgeonId = input.SurgeonId,
                TheaterId = input.TheaterId,
                BodyStructureId = input.BodyStructureId,
                MethodId = input.MethodId,
                HospitalId = input.HospitalId,
                PreOperativeAssessmentId = input.Id,
                Status = PatientSurveyStatus.Created
            };
            await _patientSurveysRepository.InsertAsync(survey);
        }

        private async Task UpdateDiagnosticId(Guid id, int? diagnosticReportId)
        {
            var poap = await Repository.GetAll()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            poap.DiagnosticReportId = diagnosticReportId;
            await Repository.UpdateAsync(poap);
        }

        private async Task<List<Guid>> GetRiskFactorList(Guid preoperativeAssessmentId, int diagnosticReportId)
        {
            var list = await _poapRiskFactorRepository.GetAll()
                .Where(e => e.PreoperativeAssessmentId == preoperativeAssessmentId && e.DiagnosticReportId == diagnosticReportId)
                .Select(e => e.RiskId)
                .ToListAsync();

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poapId"></param>
        /// <param name="poapProcedures"></param>
        /// <returns></returns>
        private async Task SavePoapProcedureDevice(
            Guid poapId,
            IEnumerable<PoapProcedureDto> poapProcedures,
            List<PoapProcedureDevicesDto> poapProcedureDevicesDtos)
        {
            if (poapProcedures != null)
            {
                if (poapProcedures.Count() > 0)
                {
                    foreach (var proc in poapProcedures)
                    {
                        var poapProcDevices = poapProcedureDevicesDtos.Where(_ => _.PoapProcedureId == proc.Id || _.SnomedId == proc.SnomedId);
                        if (poapProcDevices != null)
                        {
                            if (poapProcDevices.Count() > 0)
                            {
                                proc.PoapProcedureDevices = poapProcDevices;

                                IList<Entities.PoapProcedureDevices> existing = new List<Entities.PoapProcedureDevices>();
                                IList<CaseMix.Entities.PoapProcedureDevices> poapProcedureDevices
                                   = new List<CaseMix.Entities.PoapProcedureDevices>();
                                
                                await _devicePoapProcDevRepository.DeleteAsync(e =>
                                    e.PreOperativeAssessmentId == poapId &&
                                    e.PoapProcedureId == proc.Id);

                                foreach (var procd in proc.PoapProcedureDevices)
                                {
                                    CaseMix.Entities.PoapProcedureDevices poapProcedureDevice = new CaseMix.Entities.PoapProcedureDevices();
                                    poapProcedureDevice.CreatedDate = DateTime.UtcNow;
                                    poapProcedureDevice.DeviceId = procd.DeviceId;
                                    poapProcedureDevice.PoapProcedureId = proc.Id ?? new Guid();
                                    poapProcedureDevice.PreOperativeAssessmentId = poapId;
                                    poapProcedureDevice.SnomedId = proc.SnomedId;
                                    poapProcedureDevice.UserId = AbpSession.UserId.Value;

                                    await _devicePoapProcDevRepository.InsertAsync(poapProcedureDevice);
                                }

                                await CurrentUnitOfWork.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="poapProcedureDevices"></param>
        ///// <param name="existingPoapProcedureDevices"></param>
        ///// <returns></returns>
        //private async Task CheckIfNotSelectedButExisted(IList<Entities.PoapProcedureDevices> poapProcedureDevices,
        //    IList<Entities.PoapProcedureDevices> existingPoapProcedureDevices)
        //{
        //    var query = existingPoapProcedureDevices.Where(_ => !poapProcedureDevices.Contains(_)).ToList();

        //    if (query != null)
        //    {
        //        if (query.Count() > 0)
        //        {
        //            foreach (var _ in query)
        //            {
        //                await _devicePoapProcDevRepository.DeleteAsync(_);
        //            }

        //            await CurrentUnitOfWork.SaveChangesAsync();
        //        }
        //    }
        //}
    }
}
