using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("Regions")]
    public class Region : Entity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
