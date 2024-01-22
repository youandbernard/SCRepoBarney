using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("PasswordResets")]
    public class PasswordReset : Entity<Guid>
    {
        public virtual DateTime DateSent { get; set; }
        public virtual bool IsResetted { get; set; }
        public virtual DateTime? ResetDate { get; set; }
        public virtual string Email { get; set; }
    }
}
