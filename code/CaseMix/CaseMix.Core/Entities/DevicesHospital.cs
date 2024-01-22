using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("deviceshospital")]
    public class DevicesHospital : Entity<int>
    {
        public int DeviceId { get; set; }

        public string HospitalId { get; set; }

        public long UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
    }
}
