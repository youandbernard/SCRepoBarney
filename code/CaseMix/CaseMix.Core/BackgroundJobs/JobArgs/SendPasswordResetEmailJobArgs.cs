using System;

namespace CaseMix.BackgroundJobs.JobArgs
{
    public class SendPasswordResetEmailJobArgs
    {
        public Guid PasswordResetId { get; set; }
        public string Email { get; set; }
    }
}
