using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("PoapProcedures")]
    public class PoapProcedure : Entity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual string ProcedureSite { get; set; }
        public virtual string Method { get; set; }
        public virtual double MeanTime { get; set; }
        public virtual double StandardDeviation { get; set; }
        public virtual double ActualTime { get; set; }
        public virtual bool IsRisk { get; set; }
        public virtual string SnomedId { get; set; }
        public virtual bool IsPatientPreparation { get; set; }
        public virtual Guid PreOperativeAssessmentId { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual DateTime? ClockStartTimestamp { get; set; }
        public virtual DateTime? ClockEndTimestamp { get; set; }

        [NotMapped]
        public bool HasSurveyNotes { get; set; }

        [NotMapped]
        public bool ShowButtonDevProc { get; set; }

        public virtual ICollection<PoapProcedureDevices> PoapProcedureDevices { get; set; }

        [ForeignKey("PreOperativeAssessmentId")]
        public PreOperativeAssessment PreOperativeAssessment { get; set; }
    }
}
