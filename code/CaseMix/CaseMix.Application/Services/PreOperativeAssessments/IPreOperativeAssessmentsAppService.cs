using Abp.Application.Services;
using CaseMix.Services.PoapRiskFactors.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData;
using CaseMix.Services.PreOperativeAssessments.Dto.SetPoapData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.PreOperativeAssessments
{
    public interface IPreOperativeAssessmentsAppService :  IAsyncCrudAppService<PreOperativeAssessmentDto, Guid, PagedPoapResultRequestDto>
    {
        Task<SetPoapDataOutputDto> SetPoapData(SetPoapDataInputDto input);
        Task<GetPoapDataOutputDto> GetPoapData(GetPoapDataInputDto input);
        Task<IEnumerable<PreOperativeAssessmentDto>> GetPoapDataByTheaterAsync(Guid TheaterId);
        Task<IEnumerable<ProcedureMethodTypeDto>> GetAllProcedureMethodTypes();
        Task UpdateAwaitingRiskCompletion(Guid id, int awaitingRiskCompletionStatus);
        Task SavePoapRiskFactors(PreOperativeAssessmentDto preOperativeAssessment);
        Task SaveUnselectedRiskFactors(Guid id, IEnumerable<DiagnosticRiskFactorsMappingDto> riskFactorMappings);
    }
}
