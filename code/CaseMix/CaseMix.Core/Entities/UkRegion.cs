using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("ukregions")]
    public class UkRegion : Entity<int>
    {
        public virtual string Name { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
