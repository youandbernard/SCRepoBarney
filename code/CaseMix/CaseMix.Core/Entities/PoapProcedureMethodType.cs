using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("PoapProcedureMethodTypes")]
    public class PoapProcedureMethodType : Entity<int>
    {
        public Guid PreOperativeAssessmentId { get; set; }
        public Guid ProcedureTypeId { get; set; }
    }
}
