using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("devicefamily")]
    public class DeviceFamily : Entity<int>
    {
        public Guid DeviceFamilyId { get; set; }
        
        public string Name { get; set; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }

        public Guid? BodyStructureGroupId { get; set; }
    }
}
