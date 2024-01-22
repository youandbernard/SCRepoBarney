using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("ProcedureMethodTypes")]
    public class ProcedureMethodType :Entity<Guid>
    {
        public virtual string Description { get; set; }
    }
}
