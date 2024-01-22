using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("ComorbidityGroups")]
    public class ComorbidityGroup : Entity<int>
    {
        public virtual string Description { get; set; }
        public virtual ICollection<Comorbidity> Comorbidities { get; set;}
        public virtual string SnomedId { get; set; }
    }
}
