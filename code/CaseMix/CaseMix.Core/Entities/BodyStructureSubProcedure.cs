using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("BodyStructureSubprocedures")]
    public class BodyStructureSubProcedure : Entity<int>
    {
        public virtual string SnomedId { get; set; }
        public virtual string Description { get; set; }
        public virtual int BodyStructureId { get; set; }
        public bool ShowButtonDevProc { get; set; }

        [ForeignKey("BodyStructureId")]
        public virtual BodyStructure BodyStructure { get; set; }
    }
}
