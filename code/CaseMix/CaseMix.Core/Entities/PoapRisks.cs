using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("PoapRisks")]
    public class PoapRisk : Entity<Guid>
    {
        public virtual string Group { get; set; }
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
        public virtual double MeanTime { get; set; }
        public virtual double StandardDeviation { get; set; }
        public virtual Guid PreOperativeAssessmentId { get; set; }
        public virtual int? DiagnosticId { get; set; }
        
        [ForeignKey("PreOperativeAssessmentId")]
        public PreOperativeAssessment PreOperativeAssessment { get; set; }
    }
}
