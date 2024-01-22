using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("BodyStructureGroups")]
    public class BodyStructureGroup : Entity<Guid>
    {
        public BodyStructureGroup()
        {
            BodyStructures = new HashSet<BodyStructure>();
        }

        public virtual string Name { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual ICollection<HospitalSpecialty> HospitalSpecialties { get; set; }
        public virtual ICollection<BodyStructure> BodyStructures { get; set; }
        public virtual ICollection<SurgeonSpecialty> SurgeonSpecialties { get; set; }
    }
}
