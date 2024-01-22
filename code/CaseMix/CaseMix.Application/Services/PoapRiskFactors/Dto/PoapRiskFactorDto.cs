using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.DiagnosticReports.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PoapRiskFactors.Dto
{
    [AutoMap(typeof(PoapRiskFactor))]
    public class PoapRiskFactorDto : EntityDto<int>
    {
        public Guid PreoperativeAssessmentId { get; set; }
        public int DiagnosticReportId { get; set; }
        public Guid RiskId { get; set; }

        public PreOperativeAssessmentDto PreOperativeAssessment { get; set; }
        public DiagnosticReportDto DiagnosticReport { get; set; }
        public PoapRiskDto PoapRisk { get; set; } 
    }
}
