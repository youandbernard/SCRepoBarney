using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper.QueryableExtensions;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using CaseMix.DomainServices.Timezone;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.PatientSurveys.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using CaseMix.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace CaseMix.Services.PatientSurveys
{
    public class PatientSurveysAppService : AsyncCrudAppService<PatientSurvey, PatientSurveyDto, Guid, PagedPatientSurveysRequestDto>, IPatientSurveysAppService
    {
        private readonly IRepository<Patient, string> _patientsRepository;
        private readonly IRepository<PatientSurveyNotes, Guid> _patientSurveyNotesrepository;
        private readonly IRepository<HospitalPatient, Guid> _hospitalPatientsRepository;
        private readonly IRepository<PoapProcedure, Guid> _poapProceduresRepository;
        private readonly IRepository<PreOperativeAssessment, Guid> _preOperativeAssessmentRepository;
        private readonly IRepository<UserDisplayCompletedSurveySetting, int> _userDisplayCompletedSurveySettingRepository;
        private readonly IRepository<BodyStructureSubProcedure, int> _bodyStructureSubProcedureRepository;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly ITimezoneDomainService _timezoneDomainService;

        public PatientSurveysAppService(
            IRepository<PatientSurvey, Guid> repository,
            IRepository<PatientSurveyNotes, Guid> patientSurveyNotesrepository,
            IRepository<Patient, string> patientsRepository,
            IRepository<HospitalPatient, Guid> hospitalPatientsRepository,
            IRepository<PoapProcedure, Guid> poapProceduresRepository,
            IRepository<PreOperativeAssessment, Guid> preOperativeAssessmentRepository,
            IRepository<BodyStructureSubProcedure, int> bodyStructureSubProcedureRepository,
            ITimezoneDomainService timezoneDomainService,
            IRepository<UserDisplayCompletedSurveySetting, int> userDisplayCompletedSurveySettingRepository,
            UserManager userManager,
            RoleManager roleManager
            ) : base(repository)
        {
            _patientsRepository = patientsRepository;
            _patientSurveyNotesrepository = patientSurveyNotesrepository;
            _hospitalPatientsRepository = hospitalPatientsRepository;
            _poapProceduresRepository = poapProceduresRepository;
            _preOperativeAssessmentRepository = preOperativeAssessmentRepository;
            _userManager = userManager;
            _timezoneDomainService = timezoneDomainService;
            _userDisplayCompletedSurveySettingRepository = userDisplayCompletedSurveySettingRepository;
            _bodyStructureSubProcedureRepository = bodyStructureSubProcedureRepository;
        }

        protected override IQueryable<PatientSurvey> CreateFilteredQuery(PagedPatientSurveysRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                .Where(e => e.PreOperativeAssessment.IsArchived == input.IsArchived)
                .Where(e => e.PreOperativeAssessment.HospitalId == input.HospitalId)
                .WhereIf(input.SurgeonId.HasValue, e => e.PreOperativeAssessment.SurgeonId == input.SurgeonId)
                .WhereIf(!input.TheaterId.IsNullOrWhiteSpace(), e => e.PreOperativeAssessment.Theater.Name == input.TheaterId)
                .WhereIf(input.BodyStructureGroupId.HasValue, e => e.PreOperativeAssessment.BodyStructure.BodyStructureGroupId == input.BodyStructureGroupId)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), e => e.PreOperativeAssessment.PatientId.ToLower().Contains(input.Keyword.ToLower())
                    || e.PatientDobYear.ToString().ToLower().Contains(input.Keyword.ToLower())
                    || e.PreOperativeAssessment.Theater.TheaterId.ToLower().Contains(input.Keyword.ToLower())
                    || e.ObserverStaffId.ToLower().Contains(input.Keyword.ToLower())
                    || e.PreOperativeAssessment.BodyStructure.BodyStructureGroup.Name.Contains(input.Keyword.ToLower())
                )
                .Include(e => e.PreOperativeAssessment)
                    .ThenInclude(e => e.BodyStructure.BodyStructureGroup)
                .Include(e => e.PreOperativeAssessment)
                    .ThenInclude(e => e.Surgeon)
                .Include(e => e.PreOperativeAssessment)
                    .ThenInclude(e => e.Procedures)
                .Include(e => e.PreOperativeAssessment.Theater);
        }

        public override async Task<PagedResultDto<PatientSurveyDto>> GetAllAsync(PagedPatientSurveysRequestDto input)
        {
            var output = await base.GetAllAsync(input);
            var cultureInfo = new CultureInfo("en-US");
            var calendar = cultureInfo.Calendar;
            var rule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            var dayOfWeek = DayOfWeek.Monday;
            var currentUser = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var roles = await _userManager.GetRolesAsync(currentUser);
            foreach (var survey in output.Items)
            {
                survey.isAdmin = roles.Contains(StaticRoleNames.Tenants.SuperAdmin);
                var surgeryDate = ConvertToLocal(survey.PreOperativeAssessment.SurgeryDate, input.Timezone);
                survey.TotalMeanTime = Math.Round(survey.PreOperativeAssessment.Procedures.Sum(e => e.MeanTime), 0);
                var weekOfYear = calendar.GetWeekOfYear(surgeryDate, rule, dayOfWeek).ToString();
                var startDate = surgeryDate.StartOfWeek(DayOfWeek.Monday);
                var endDate = startDate.AddDays(6);
                survey.WeekOfYear = $"Week {weekOfYear} ({startDate:d MMMM yyyy} - {endDate:d MMMM yyyy})";
                survey.PreOperativeAssessment.SurgeryDate = surgeryDate;
            }

            return output;
        }

        public override async Task<PatientSurveyDto> GetAsync(EntityDto<Guid> input)
        {
            var output = await Repository.GetAll()
                .Include(e => e.PreOperativeAssessment)
                    .ThenInclude(e => e.Procedures)
                .Include(e => e.PreOperativeAssessment.Patient)
                .Include(e => e.Hospital)
                .Include(e => e.PreOperativeAssessment.BodyStructure)
                    .ThenInclude(e => e.BodyStructureGroup)
                .Include(e => e.PreOperativeAssessment.Theater)
                .Where(e => e.Id == input.Id)
                .Select(e => ObjectMapper.Map<PatientSurveyDto>(e))
                .FirstOrDefaultAsync();

            return output;
        }

        public async Task<PatientSurveyDto> GetSurveyAsync(Guid surveyId, string timezone)
        {
            var output = await Repository.GetAll()
                .Include(e => e.PatientSurveyNotes)
                .Include(e => e.PreOperativeAssessment)
                    .ThenInclude(e => e.Procedures)
                    .ThenInclude(e => e.PoapProcedureDevices)
                        .ThenInclude(e => e.Device)
                         .ThenInclude(e => e.Manufacturer).AsNoTracking()
                .Include(e => e.PreOperativeAssessment.Patient)
                .Include(e => e.Hospital)
                .Include(e => e.PreOperativeAssessment.BodyStructure)
                    .ThenInclude(e => e.BodyStructureGroup)
                .Include(e => e.PreOperativeAssessment.Theater)
                .Where(e => e.Id == surveyId)
                .Select(e => ObjectMapper.Map<PatientSurveyDto>(e))
                .FirstOrDefaultAsync();

            output.PreOperativeAssessment.SurgeryDate = ConvertToLocal(output.PreOperativeAssessment.SurgeryDate, timezone);

            foreach (var procedure in output.PreOperativeAssessment.Procedures)
            {
                if (procedure.ClockStartTimestamp.HasValue && procedure.ClockEndTimestamp.HasValue)
                {
                    procedure.ClockStartTimestamp = ConvertToLocal(procedure.ClockStartTimestamp.Value, timezone);
                    procedure.ClockEndTimestamp = ConvertToLocal(procedure.ClockStartTimestamp.Value, timezone);
                }
            }

            if (output.DateStart.HasValue)
            {
                output.StartTime = ConvertToLocal(output.DateStart.Value, timezone).ToString("hh:mm tt");
                output.DateStart = ConvertToLocal(output.DateStart.Value, timezone);
            }

            output.PreOperativeAssessment.Procedures = output.PreOperativeAssessment.Procedures.Where(e => !e.IsRisk).ToList();

            if (output.PreOperativeAssessment.Procedures != null)
            {
                foreach(var proc in output.PreOperativeAssessment.Procedures)
                {
                    var isShows = _bodyStructureSubProcedureRepository.GetAll()
                        .Where(_ => _.SnomedId == proc.SnomedId && _.BodyStructureId == output.BodyStructureId).ToList();

                    var isShow = isShows.Where(_ => _.ShowButtonDevProc == true && _.BodyStructureId == output.BodyStructureId).FirstOrDefault();
                    proc.ShowButtonDevProc = isShow != null? isShow.ShowButtonDevProc : false;
                }
            }

            if (output.PatientSurveyNotes != null)
            {
                if (output.PatientSurveyNotes.Count() > 0)
                {
                    if (output.PreOperativeAssessment.Procedures != null)
                    {
                        foreach (var proc in output.PreOperativeAssessment.Procedures)
                        {
                            proc.HasSurveyNotes = output.PatientSurveyNotes.Any(e => e.PoapProcedureId == proc.Id);
                        }
                    }
                }
            }

            return output;
        }

        public async Task<IEnumerable<UserDto>> GetSurgeons(string hospitalId)
        {
            var surgeons = await Repository.GetAll()
                .Where(e => e.PreOperativeAssessment.Surgeon != null && e.PreOperativeAssessment.HospitalId == hospitalId)
                .Select(e => ObjectMapper.Map<UserDto>(e.PreOperativeAssessment.Surgeon))
                .Distinct()
                .ToListAsync();

            return surgeons;
        }

        public async Task SaveSurveyStartTime(Guid surveyId, DateTime dateStart, string timezone)
        {
            var survey = Repository.Get(surveyId);
            var dateStartUTC = _timezoneDomainService.ConvertToUtc(dateStart, timezone);

            if (survey != null)
            {
                survey.StartTime = dateStartUTC.ToString("hh:mm tt");
                survey.DateStart = dateStartUTC;
                survey.Status = PatientSurveyStatus.Ongoing;
                survey.CreatedDate = DateTime.UtcNow;

                Console.WriteLine($"Saving Survey Date Start: " + survey.DateStart + $" - Version:" + GetProductVersion());
                await Repository.UpdateAsync(survey);

            }
        }

        public async Task<bool> SaveSurveyNotes(List<PatientSurveyNotesDto> patientSurveyNotesDto, Guid patientSurveyId)
        {
            bool ret = false;
            try
            {
                var patientSurvey = await Repository.GetAll().Where(e => e.Id == patientSurveyId).FirstOrDefaultAsync();
                if (patientSurvey == null)
                    throw new UserFriendlyException("Survey ID not found.");

                //delete existing notes but removed from display: for edit
                await this.DeleteSurveyNotesAsync(patientSurveyNotesDto, patientSurveyId);
                if (patientSurveyNotesDto != null)
                {
                    if (patientSurveyNotesDto.Count > 0)
                    {
                        foreach (var note in patientSurveyNotesDto)
                        {
                            var surveyNote = new PatientSurveyNotes()
                            {
                                NoteSeries = note.NoteSeries,
                                NoteTabs = note.NoteTabs,
                                NoteDescription = note.NoteDescription,
                                PatientSurveyId = patientSurvey.Id,
                                PoapProcedureId = note.PoapProcedureId,
                                PreOperativeAssessmentId = note.PreOperativeAssessmentId,
                                CreatedBy = AbpSession.UserId.Value,
                                CreatedDate = DateTime.UtcNow
                            };

                            var exists = await _patientSurveyNotesrepository.GetAll()
                                .Where(e => e.PatientSurveyId == patientSurveyId &&
                                    e.PoapProcedureId == note.PoapProcedureId &&
                                    e.PreOperativeAssessmentId == note.PreOperativeAssessmentId &&
                                    e.NoteSeries == surveyNote.NoteSeries &&
                                    e.NoteTabs == surveyNote.NoteTabs
                                ).FirstOrDefaultAsync();

                            if (exists != null)
                            {
                                exists.NoteDescription = surveyNote.NoteDescription;
                                exists.UpdatedBy = AbpSession.UserId.Value;
                                exists.UpdatedDate = DateTime.UtcNow;
                            }
                            else
                            {
                                await _patientSurveyNotesrepository.InsertAsync(surveyNote);
                            }

                            await CurrentUnitOfWork.SaveChangesAsync();
                        }

                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        private async Task DeleteSurveyNotesAsync(List<PatientSurveyNotesDto> patientSurveyNotesDto, Guid patientSurveyId)
        {
            if (patientSurveyNotesDto.Count == 0)
            {
                await _patientSurveyNotesrepository.DeleteAsync(d => d.PatientSurveyId == patientSurveyId);
            }
            else
            {
                var poapId = patientSurveyNotesDto[0].PreOperativeAssessmentId;
                var existingPoap = patientSurveyNotesDto.Select(s => s.PoapProcedureId).Distinct().ToList();
                var toRemovePoapNotes = await _patientSurveyNotesrepository.GetAll()
                                             .Where(e => e.PatientSurveyId == patientSurveyId &&
                                                         e.PreOperativeAssessmentId == poapId &&
                                                         !existingPoap.Any(c => c == e.PoapProcedureId)).Select(s => s.PoapProcedureId).Distinct().ToListAsync();
                if (toRemovePoapNotes.Count > 0)
                {
                    await _patientSurveyNotesrepository.DeleteAsync(d => d.PatientSurveyId == patientSurveyId && d.PreOperativeAssessmentId == poapId && toRemovePoapNotes.Any(c => c == d.PoapProcedureId));
                }
            }
        }

        public async Task<IEnumerable<string>> GetTheaterIds(string hospitalId)
        {
            var theaterIds = await Repository.GetAll()
                .Where(e => e.PreOperativeAssessment.HospitalId == hospitalId)
                .Select(e => e.PreOperativeAssessment.Theater.Name)
                .Distinct()
                .ToListAsync();

            return theaterIds;
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetBodyStructureGroups(string hospitalId)
        {
            var bodyStructureGroups = await Repository.GetAll()
                .Where(e => e.PreOperativeAssessment.BodyStructure != null && e.PreOperativeAssessment.BodyStructure.BodyStructureGroup != null && e.PreOperativeAssessment.HospitalId == hospitalId)
                .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e.PreOperativeAssessment.BodyStructure.BodyStructureGroup))
                .Distinct()
                .ToListAsync();

            return bodyStructureGroups;
        }

        public override Task<PatientSurveyDto> UpdateAsync(PatientSurveyDto input)
        {
            // Update display orders
            var procedures = _poapProceduresRepository.GetAll().Where(s => s.PreOperativeAssessmentId == input.PreOperativeAssessmentId).ToList();

            var i = 0;
            foreach (var procedureDto in input.PreOperativeAssessment.Procedures)
            {
                var toUpdateProcedure = procedures.Where(s => s.Id == procedureDto.Id).FirstOrDefault();
                if (toUpdateProcedure == null) continue;
                toUpdateProcedure.DisplayOrder = i++;
                _poapProceduresRepository.Update(toUpdateProcedure);
            }

            if (input.PreOperativeAssessment.IsArchived)
            {
                var preOperativeAssessment = _preOperativeAssessmentRepository.Get(input.PreOperativeAssessmentId);
                preOperativeAssessment.IsArchived = true;
                preOperativeAssessment.AwaitingRiskCompletion = (int)PoapAwaitingRiskCompletionStatus.True;
                _preOperativeAssessmentRepository.Update(preOperativeAssessment);
            }

            input.BodyStructure = null;
            input.Surgeon = null;
            input.Hospital = null;
            input.PreOperativeAssessment = null;

            return base.UpdateAsync(input);
        }

        public async Task UpdatePatientSurveyAsync(PatientSurveyDto input)
        {
            var procedures = _poapProceduresRepository.GetAll().Where(s => s.PreOperativeAssessmentId == input.PreOperativeAssessmentId).ToList();

            var i = 0;
            foreach (var procedureDto in input.PreOperativeAssessment.Procedures)
            {
                var toUpdateProcedure = procedures.Where(s => s.Id == procedureDto.Id).FirstOrDefault();
                if (toUpdateProcedure == null) continue;
                toUpdateProcedure.DisplayOrder = i++;
                _poapProceduresRepository.Update(toUpdateProcedure);
            }

            if (input.PreOperativeAssessment.IsArchived)
            {
                var preOperativeAssessment = _preOperativeAssessmentRepository.Get(input.PreOperativeAssessmentId);
                preOperativeAssessment.IsArchived = true;
                preOperativeAssessment.AwaitingRiskCompletion = (int)PoapAwaitingRiskCompletionStatus.True;
                _preOperativeAssessmentRepository.Update(preOperativeAssessment);
            }

            input.BodyStructure = null;
            input.Surgeon = null;
            input.Hospital = null;
            input.PreOperativeAssessment = null;
            var patientSurvey = await base.Repository.FirstOrDefaultAsync(e => e.Id == input.Id);

            if (!input.IsReplicate)
            {
                patientSurvey.Status = PatientSurveyStatus.Completed;
                patientSurvey.ObserverNotes = input.ObserverNotes;
            }

            Console.WriteLine($"Saving Survey Date Start: " + patientSurvey.DateStart + $" - Version:" + GetType().Assembly.GetName().Version.ToString());
            await base.Repository.UpdateAsync(patientSurvey);
        }

        public async Task UpdateNotes(Guid surveyId, string observerNotes)
        {
            var patientSurvey = await base.Repository.FirstOrDefaultAsync(e => e.Id == surveyId);
            patientSurvey.ObserverNotes = observerNotes;
            Console.WriteLine($"Saving Survey Observer Notes: " + patientSurvey.DateStart + $" - Version:" + GetType().Assembly.GetName().Version.ToString());
            await base.Repository.UpdateAsync(patientSurvey);
        }

        public async Task UpdateProcedureActualTime(Guid poapProcedureId, double actualTime, DateTime? clockStartTimestamp, DateTime? clockEndTimestamp, string timezone)
        {

            var poapProcedure = await _poapProceduresRepository.GetAsync(poapProcedureId);
            var patientSurvey = await base.Repository.FirstOrDefaultAsync(e => e.PreOperativeAssessmentId == poapProcedure.PreOperativeAssessmentId);
            poapProcedure.ActualTime = actualTime;
            poapProcedure.ClockStartTimestamp = clockStartTimestamp.HasValue ? _timezoneDomainService.ConvertToUtc(clockStartTimestamp.Value, timezone) : clockStartTimestamp;
            poapProcedure.ClockEndTimestamp = clockEndTimestamp.HasValue ? _timezoneDomainService.ConvertToUtc(clockEndTimestamp.Value, timezone) : clockEndTimestamp;
            await _poapProceduresRepository.UpdateAsync(poapProcedure);

            if (patientSurvey != null)
            {
                patientSurvey.Status = PatientSurveyStatus.Ongoing;
                await base.Repository.UpdateAsync(patientSurvey);
            }
        }

        public async Task UpateProceduresDisplayOrder(Guid PreOperativeAssessmentId, IEnumerable<PoapProcedureDto> inputs)
        {
            var procedures = await _poapProceduresRepository.GetAll().Where(s => s.PreOperativeAssessmentId == PreOperativeAssessmentId).ToListAsync();

            foreach (var procedure in inputs)
            {
                var toUpdateProcedure = procedures.Where(s => s.Id == procedure.Id).FirstOrDefault();
                if (toUpdateProcedure != null && toUpdateProcedure.DisplayOrder != procedure.DisplayOrder)
                {
                    toUpdateProcedure.DisplayOrder = procedure.DisplayOrder;
                    _poapProceduresRepository.Update(toUpdateProcedure);
                }
            }
        }

        [HttpPost]
        public async Task ReplicateProcedure(Guid poapProcedureId, Guid preOperativeAssessmentId)
        {
            var procedures = _poapProceduresRepository.GetAll().Where(s => s.PreOperativeAssessmentId == preOperativeAssessmentId).ToList();
            var toReplicatedProcedure = procedures.Where(s => s.Id == poapProcedureId).FirstOrDefault();
            int displayOrder = toReplicatedProcedure.DisplayOrder + 1;

            // Update procedure display orders
            // todo: Create batch update and insert
            //
            var toUpdateProcedures = procedures.Where(s => s.DisplayOrder >= displayOrder).ToList();
            foreach (var toUpdateProcedure in toUpdateProcedures)
            {
                toUpdateProcedure.DisplayOrder += 1;
                await _poapProceduresRepository.UpdateAsync(toUpdateProcedure);
            }

            // Insert the replicate procedure
            //
            var newProcedure = new PoapProcedure();
            newProcedure.Name = toReplicatedProcedure.Name;
            newProcedure.DisplayOrder = toReplicatedProcedure.DisplayOrder + 1;
            newProcedure.ProcedureSite = toReplicatedProcedure.ProcedureSite;
            newProcedure.Method = toReplicatedProcedure.Method;
            newProcedure.MeanTime = 0;
            newProcedure.StandardDeviation = toReplicatedProcedure.StandardDeviation;
            newProcedure.ActualTime = 0; // set to 0 again
            newProcedure.IsRisk = toReplicatedProcedure.IsRisk;
            newProcedure.SnomedId = toReplicatedProcedure.SnomedId;
            newProcedure.ParentId = toReplicatedProcedure.Id;
            newProcedure.PreOperativeAssessmentId = toReplicatedProcedure.PreOperativeAssessmentId;
            await _poapProceduresRepository.InsertAsync(newProcedure);
        }

        [HttpPost]
        public async Task SaveDisplayCompletedSurveySetting(bool displayCompletedSurveySetting)
        {
            var displaySetting = await _userDisplayCompletedSurveySettingRepository.FirstOrDefaultAsync(e => e.UserId == AbpSession.UserId.Value);

            if (displaySetting == null)
            {
                displaySetting = new UserDisplayCompletedSurveySetting();
                displaySetting.UserId = AbpSession.UserId.Value;
            }

            displaySetting.DisplayCompletedSurvey = displayCompletedSurveySetting;

            await _userDisplayCompletedSurveySettingRepository.InsertOrUpdateAsync(displaySetting);
        }

        [HttpGet]
        public async Task<UserDisplayCompletedSurveySettingDto> GetDisplayCompletedSurveySetting()
        {
            var displaySetting = await _userDisplayCompletedSurveySettingRepository.GetAll()
                .Where(e => e.UserId == AbpSession.UserId.Value)
                .Select(e => ObjectMapper.Map<UserDisplayCompletedSurveySettingDto>(e))
                .FirstOrDefaultAsync();

            if (displaySetting == null)
            {
                displaySetting = new UserDisplayCompletedSurveySettingDto();
            }

            return displaySetting;
        }

        private DateTime ConvertToUtc(DateTime dateStart, string timezone)
        {
            var timeZone = TZConvert.GetTimeZoneInfo(timezone);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone.Id);
            var dateStartUtc = TimeZoneInfo.ConvertTimeToUtc(dateStart, timeZoneInfo);

            return dateStartUtc;
        }

        private DateTime ConvertToLocal(DateTime dateStart, string timezone)
        {
            var timeZone = TZConvert.GetTimeZoneInfo(timezone);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone.Id);
            var dateStartUtc = TimeZoneInfo.ConvertTimeFromUtc(dateStart, timeZoneInfo);

            return dateStartUtc;
        }

        private string GetProductVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fileVersionInfo.ProductVersion;
        }
    }
}
