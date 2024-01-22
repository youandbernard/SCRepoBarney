using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("HospitalSpecialties")]
    public class HospitalSpecialty : Entity<int>
    {
        public virtual string HospitalId { get; set; }
        public virtual Guid BodyStructureGroupId { get; set; }
    }
}
