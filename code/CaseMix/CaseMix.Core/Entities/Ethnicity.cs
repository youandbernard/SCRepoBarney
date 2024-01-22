using Abp.Domain.Entities;
using CaseMix.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("Ethnicities")]
    public class Ethnicity : Entity<int>
    {
        public virtual string Description { get; set; }

        public virtual ICollection<PreOperativeAssessment> PreOperativeAssessments { get; set; }
    }
}
