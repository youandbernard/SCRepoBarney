using Abp.Domain.Entities;
using CaseMix.Authorization.Users;
using CaseMix.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("PatientSurveys")]
    public class PatientSurvey : Entity<Guid>
    {
        public virtual string PatientId { get; set; }
        public virtual string HospitalId { get; set; }
        public virtual int BodyStructureId { get; set; }
        public virtual long SurgeonId { get; set; }
        public virtual Guid PreOperativeAssessmentId { get; set; }

        public virtual int? PatientDobYear { get; set; }
        public virtual DateTime? SurgeryDate { get; set; }
        public virtual string EndTime { get; set; }
        public virtual string NextLocation { get; set; }
        public virtual Guid TheaterId { get; set; }
        public virtual string MethodId { get; set; }
        public virtual string ObserverNotes { get; set; }
        public virtual string ObserverStaffId { get; set; }
        public virtual string ObserverSignature { get; set; }
        public virtual string StartTime { get; set; }
        public virtual PatientSurveyStatus? Status { get; set; }
        public virtual DateTime? DateStart { get; set; }
        public virtual DateTime? CreatedDate { get; set; }

        public virtual ICollection<PatientSurveyNotes> PatientSurveyNotes { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        
        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }
        
        [ForeignKey("BodyStructureId")]
        public BodyStructure BodyStructure { get; set; }
        
        [ForeignKey("SurgeonId")]
        public User Surgeon { get; set; }

        [ForeignKey("PreOperativeAssessmentId")]
        public PreOperativeAssessment PreOperativeAssessment { get; set; }

        [ForeignKey("TheaterId")]
        public Theater Theater { get; set; }
    }
}
