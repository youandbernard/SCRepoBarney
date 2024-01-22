using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PatientSurveys.Dto
{
    [AutoMap(typeof(PatientSurveyNotes))]
    public class PatientSurveyNotesDto : EntityDto<Guid>
    {
        public Guid PatientSurveyId { get; set; }
        public int NoteSeries { get; set; }
        public int NoteTabs { get; set; }
        public string NoteDescription { get; set; }
        public Guid PreOperativeAssessmentId { get; set; }
        public Guid PoapProcedureId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual PatientSurveyDto PatientSurvey { get; set; }
    }
}
