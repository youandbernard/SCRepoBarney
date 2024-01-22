using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("Instruments")]
    public class Instrument : Entity<int>
    {
        public string UserCode { get; set; }
        public string Description { get; set; }
        public double AveWeigthKg { get; set; }
        public double EmbodiedCarbonPerKg { get; set; }
    }
}
