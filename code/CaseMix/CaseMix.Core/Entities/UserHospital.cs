using Abp.Domain.Entities;
using CaseMix.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("UserHospitals")]
    public class UserHospital : Entity<Guid>
    {
        public virtual long UserId { get; set; }
        public virtual string HospitalId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }
    }
}
