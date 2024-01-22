using Abp.Domain.Entities;
using CaseMix.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("Patients")]
    public class Patient : Entity<string>
    {
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual GenderType? Gender { get; set; }
        public virtual bool Active { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Deceased { get; set; }
        public virtual string Address { get; set; }
        public virtual string EthnicCategory { get; set; }
    }
}
