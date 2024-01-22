using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("ReportingSettings")]
    public class ReportingSetting : Entity<int>
    {
        public virtual string HospitalId { get; set; }
        public virtual bool IsEnabled { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }
    }
}
