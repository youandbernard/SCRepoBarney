using Abp.Domain.Entities;
using CaseMix.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("SurgeonSpecialties")]
    public class SurgeonSpecialty : Entity<int>
    {
        public virtual long SurgeonId { get; set; }
        public virtual Guid BodyStructureGroupId { get; set; }

        [ForeignKey("SurgeonId")]
        public virtual User User { get; set; }
    }
}
