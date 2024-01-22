using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Services.PoapProcedureDevices.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Services.PreOperativeAssessments.Dto
{
    [AutoMap(typeof(PoapProcedure))]
    public class PoapProcedureDto : EntityDto<Guid?>
    {
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string ProcedureSite { get; set; }
        public string Method { get; set; }
        public double MeanTime { get; set; }
        public double StandardDeviation { get; set; }
        public double ActualTime { get; set; }
        public bool IsRisk { get; set; }
        public bool IsPatientPreparation { get; set; }
        public string SnomedId { get; set; }
        public virtual Guid? ParentId { get; set; }
        public Guid PreOperativeAssessmentId { get; set; }
        public DateTime? ClockStartTimestamp { get; set; }
        public DateTime? ClockEndTimestamp { get; set; }

        [NotMapped]
        public bool HasSurveyNotes { get; set; }

        [NotMapped]
        public bool ShowButtonDevProc { get; set; }

        public IEnumerable<PoapProcedureDevicesDto> PoapProcedureDevices { get; set; }
    }
}
