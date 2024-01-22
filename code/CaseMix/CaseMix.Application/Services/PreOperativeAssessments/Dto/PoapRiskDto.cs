using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;

namespace CaseMix.Services.PreOperativeAssessments.Dto
{
    [AutoMap(typeof(PoapRisk))]
    public class PoapRiskDto : EntityDto<Guid?>
    {
        public string Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public double MeanTime { get; set; }
        public double StandardDeviation { get; set; }
        public Guid PreOperativeAssessmentId { get; set; }
        public string SnomedId { get; set; }
        public int? DiagnosticId { get; set; }
    }
}
