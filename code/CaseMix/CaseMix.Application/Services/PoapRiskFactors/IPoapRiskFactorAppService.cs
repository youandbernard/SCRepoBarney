using CaseMix.Services.PoapRiskFactors.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.PoapRiskFactors
{
    public interface IPoapRiskFactorAppService
    {
        Task<IEnumerable<PoapRiskFactorDto>> GetAllAsync();
    }
}
