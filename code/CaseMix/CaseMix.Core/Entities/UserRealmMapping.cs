using Abp.Domain.Entities;
using CaseMix.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("UserRealmMappings")]
    public class UserRealmMapping : Entity<Guid>
    {
        public virtual long? UserId { get; set; }
        public virtual Guid? RegionId { get; set; }
        public virtual string HospitalId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RegionId")]
        public Region Region  { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }
    }
}
