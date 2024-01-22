using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("Hospitals")]
    public class Hospital : Entity<string>
    {
        public virtual string Name { get; set; }
        public virtual string ReportUrl { get; set; }
        public virtual Guid? TrustId { get; set; }
        public virtual string Postcode { get; set; }
        public virtual bool ActiveDevMgt { get; set; }
        public virtual bool? ShowButtonDevProc { get; set; }

        public ReportingSetting Setting { get; set; }
        public RiskMappingSetting RiskMappingSetting { get; set; }

        public virtual ICollection<SpecialtyInfo> SpecialtyInfos { get; set; }
    }
}
