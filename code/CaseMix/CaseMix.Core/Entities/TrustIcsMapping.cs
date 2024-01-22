using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("trusticsmappings")]
    public class TrustIcsMapping : Entity<Guid>
    {
        public Guid? RegionId { get; set; }
        public int IntegratedCareSystemId { get; set; }

        [ForeignKey("IntegratedCareSystemId")]
        public IntegratedCareSystem IntegratedCareSystem { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }
    }
}
