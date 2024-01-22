using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("poapproceduredevices")]
    public class PoapProcedureDevices : Entity<int>
    {
        public Guid PreOperativeAssessmentId { get; set; }
        public Guid PoapProcedureId { get; set; }
        public int DeviceId { get; set; }
        public string SnomedId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
    }
}
