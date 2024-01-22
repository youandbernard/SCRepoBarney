using Abp.Domain.Entities;
using CaseMix.Authorization.Users;
using CaseMix.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("PreOperativeAssessments")]
    public class PreOperativeAssessment : Entity<Guid>
    {
        public PreOperativeAssessment()
        {
            Risks = new HashSet<PoapRisk>();
            Procedures = new HashSet<PoapProcedure>();
        }

        public virtual string SurgeonName { get; set; }
        public virtual SurgeonExperienceType? SurgeonExperience { get; set; }
        public virtual string AnesthetistName { get; set; }
        public virtual int? DateOfBirthYear { get; set; }
        public virtual GenderType? Gender { get; set; }
        public virtual StatusType? Status { get; set; }
        public virtual DateTime AssessmentDate { get; set; }
        public virtual DateTime SurgeryDate { get; set; }
        public virtual Guid TheaterId { get; set; }
        public virtual string MethodId { get; set; }
        public virtual string HospitalId { get; set; }
        public virtual string PatientId { get; set; }
        public virtual long SurgeonId { get; set; }
        public virtual int BodyStructureId { get; set; }
        public virtual int? EthnicityId { get; set; }
        public virtual bool? IsSmoker { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual bool IsSelectAll { get; set; }
        public virtual Guid? BodyStructureGroupId { get; set; }
        public virtual ICollection<PoapRisk> Risks { get; set; }
        public virtual ICollection<PoapProcedure> Procedures { get; set; }
        public int AwaitingRiskCompletion { get; set; }
        public int? DiagnosticReportId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        [ForeignKey("SurgeonId")]
        public User Surgeon { get; set; }

        [ForeignKey("BodyStructureId")]
        public BodyStructure BodyStructure { get; set; }

        [ForeignKey("EthnicityId")]
        public Ethnicity Ethnicity { get; set; }

        [ForeignKey("TheaterId")]
        public Theater Theater { get; set; }

        [ForeignKey("DiagnosticReportId")]
        public DiagnosticReport DiagnosticReport { get; set; }
    }
}
