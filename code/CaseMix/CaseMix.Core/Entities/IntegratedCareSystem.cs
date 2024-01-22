using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("integratedcaresystems")]
    public class IntegratedCareSystem : Entity<int>
    {
        public string Name { get; set; }
        public int UkRegionId { get; set; }

        [ForeignKey("UkRegionId")]
        public UkRegion UkRegion { get; set; }
    }
}
