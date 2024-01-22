using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("PoapInstrumentPacks")]
    public class PoapInstrumentPack : Entity<int>
    {
        public Guid PoapId { get; set; }
        public Guid InstrumentPackId { get; set; }
        public int InstrumentId { get; set; }
        public string UserCode { get; set; }
        public double Quantity { get; set; }
        public double EmbodiedCarbon { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
