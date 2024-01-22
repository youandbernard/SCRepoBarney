using Abp.Domain.Entities;
using CaseMix.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("deviceprocedures")]
    public class DeviceProcedure : Entity<int>
    {
        public int DeviceId { get; set; }
        public Guid BodyStructureGroupId { get; set; }  
        public int? BodyStructureId { get; set; }
        public int? BodyStructureProcId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SnomedID { get; set; }
        public string HospitalId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("BodyStructureGroupId")]
        public virtual BodyStructureGroup BodyStructureGroup { get; set; }

        [ForeignKey("BodyStructureId")]
        public virtual BodyStructure BodyStructure { get; set; }

        [ForeignKey("BodyStructureProcId")]
        public virtual BodyStructureSubProcedure BodyStructureSubProcedure { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

    }
}
