using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CaseMix.Entities
{
    [Table("Theaters")]
    public class Theater : Entity<Guid>
    {
        public virtual string TheaterId { get; set; }
        public virtual string Name { get; set; }
        public virtual string HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }
    }
}
