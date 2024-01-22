using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.PatientSurveys.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using CaseMix.Users.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.PatientSurveys
{
    public interface IPatientSurveysAppService : IAsyncCrudAppService<PatientSurveyDto, Guid, PagedPatientSurveysRequestDto>
    {
        Task<IEnumerable<UserDto>> GetSurgeons(string hospitalId);
        Task<IEnumerable<string>> GetTheaterIds(string hospitalId);
        Task<IEnumerable<BodyStructureGroupDto>> GetBodyStructureGroups(string hospitalId);
        Task UpdateProcedureActualTime(Guid poapProcedureId, double actualTime, DateTime? clockStartTimestamp, DateTime? clockEndTimestamp, string timezone);
        Task UpateProceduresDisplayOrder(Guid PreOperativeAssessmentId, IEnumerable<PoapProcedureDto> inputs);
        Task UpdatePatientSurveyAsync(PatientSurveyDto input);
        Task SaveSurveyStartTime(Guid surveyId, DateTime dateStart, string timezone);
        Task<bool> SaveSurveyNotes(List<PatientSurveyNotesDto> patientSurveyNotesDto, Guid patientSurveyId);
        Task<PatientSurveyDto> GetSurveyAsync(Guid surveyId, string timezone);
        Task<UserDisplayCompletedSurveySettingDto> GetDisplayCompletedSurveySetting();
        Task SaveDisplayCompletedSurveySetting(bool displayCompletedSurveySetting);
        Task UpdateNotes(Guid surveyId, string observerNotes);
    }
}
