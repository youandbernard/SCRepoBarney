using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("BodyStructureQueries")]
    public class BodyStructureQuery : Entity<Guid>
    {
        public virtual string Title { get; set; }
        public virtual string Query { get; set; }
        public virtual string QuerySimplified { get; set; }
        public virtual int QueryOrder { get; set; }
        public virtual int BodyStructureId { get; set; }

        [ForeignKey("BodyStructureId")]
        public BodyStructure BodyStructure { get; set; }
    }
}
