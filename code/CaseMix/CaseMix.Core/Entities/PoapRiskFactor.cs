using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("PoapRiskFactor")]
    public class PoapRiskFactor : Entity<int>
    {
        public virtual Guid PreoperativeAssessmentId { get; set; }
        public virtual int DiagnosticReportId { get; set; }
        public virtual Guid RiskId { get; set; }
    }
}
