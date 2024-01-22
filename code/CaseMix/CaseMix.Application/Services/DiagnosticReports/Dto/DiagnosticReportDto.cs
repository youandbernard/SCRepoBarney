using Abp.Application.Services.Dto;
using AutoMapper;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.DiagnosticReports.Dto
{
    [AutoMap(typeof(DiagnosticReport))]
    public class DiagnosticReportDto : EntityDto<int>
    {
        public string Subject { get; set; }
        public DiagnosticReportStatus Status { get; set; }
        public DateTime Effective { get; set; }
        public string Conclusion { get; set; }
        public string ConclusionCode { get; set; }
    }
}
