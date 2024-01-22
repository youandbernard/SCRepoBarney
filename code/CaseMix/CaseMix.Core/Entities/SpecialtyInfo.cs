using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("specialtyinfo")]
    public class SpecialtyInfo: Entity<int>
    {
        public Guid SpecialtyId { get; set; }

        public string HospitalId { get; set; }

        public string LicenseDesc { get; set; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
