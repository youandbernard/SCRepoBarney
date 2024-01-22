using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("BodyStructures")]
    public class BodyStructure : Entity<int>
    {
        public BodyStructure()
        {
            BodyStructureQueries = new HashSet<BodyStructureQuery>();
            BodyStructureSubProcedures = new HashSet<BodyStructureSubProcedure>();
        }

        public virtual string Description { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual Guid BodyStructureGroupId { get; set; }

        public virtual ICollection<BodyStructureQuery> BodyStructureQueries { get; set; }
        public virtual ICollection<BodyStructureSubProcedure> BodyStructureSubProcedures { get; set; }

        [ForeignKey("BodyStructureGroupId")]
        public BodyStructureGroup BodyStructureGroup { get; set; }
    }
}
