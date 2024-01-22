using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("Countries")]
    public class Country : Entity<int>
    {
        public virtual string Name { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
