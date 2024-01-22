using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("InstrumentPacks")]
    public class InstrumentPack : Entity<int>
    {
        public Guid InstrumentPackId { get; set; }
        public string PackName { get; set; }
        public int PackType { get; set; }
        public double? EmbodiedCarbon { get; set; }
        public string Specialty { get; set; }
        public string Company { get; set; }
    }
}
