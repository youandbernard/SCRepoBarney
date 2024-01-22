using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("HospitalPatients")]
    public class HospitalPatient : Entity<Guid>
    {
        public string HospitalId { get; set; }
        public string PatientId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
