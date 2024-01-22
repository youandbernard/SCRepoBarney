using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("Comorbidities")] 
    public class Comorbidity : Entity<int>
    { 
        public string SnomedId { get; set; }
        public string Description { get; set; }
        public int ComorbidityGroupId { get; set; }
    }
}
