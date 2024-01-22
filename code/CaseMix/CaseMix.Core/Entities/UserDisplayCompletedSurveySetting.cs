using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("UserDisplayCompletedSurveySettings")]
    public class UserDisplayCompletedSurveySetting : Entity<int>
    {
        public virtual long UserId { get; set; }
        public virtual bool DisplayCompletedSurvey { get; set; }
    }
}
