using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("PatientSurveysNotes")]
    public class PatientSurveyNotes : Entity<Guid>
    {
        public virtual Guid PatientSurveyId { get; set; }
        public virtual int NoteSeries { get; set; }
        public virtual int NoteTabs { get; set; }
        public virtual string NoteDescription { get; set; }
        public virtual Guid PreOperativeAssessmentId { get; set; }
        public virtual Guid PoapProcedureId { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual long CreatedBy { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
        public virtual long? UpdatedBy { get; set; }

        [ForeignKey("PatientSurveyId")]
        public virtual PatientSurvey PatientSurvey { get; set; }
    }
}
