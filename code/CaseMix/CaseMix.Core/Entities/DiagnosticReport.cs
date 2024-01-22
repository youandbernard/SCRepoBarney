using Abp.Domain.Entities;
using CaseMix.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Entities
{
    public class DiagnosticReport : Entity<int>
    {
        public virtual string Subject { get; set; }
        public virtual DiagnosticReportStatus Status { get; set; }
        public virtual DateTime Effective { get; set; }
        public virtual string Conclusion { get; set; }
        public virtual string ConclusionCode { get; set; }
    }
}
