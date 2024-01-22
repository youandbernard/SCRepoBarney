using Abp.Configuration;
using CaseMix.BackgroundJobs.JobArgs;
using CaseMix.Configuration;
using CaseMix.Core.Shared.Services;

namespace CaseMix.BackgroundJobs
{
    public class SendPasswordResetEmailJob : CaseMixBackgroundJobBase<SendPasswordResetEmailJobArgs>
    {
        private readonly ISettingManager _settingManager;
        private readonly IEmailService _emailService;

        public SendPasswordResetEmailJob(ISettingManager settingManager, IEmailService emailService)
        {
            _settingManager = settingManager;
            _emailService = emailService;
        }

        public override void Execute(SendPasswordResetEmailJobArgs args)
        {
            string clientRootAddress = _settingManager.GetSettingValue(AppSettingNames.App_ClientRootAddress);
            string registrationLink = $"{clientRootAddress}account/complete-reset-password/{args.PasswordResetId}";
            string subject = L("PasswordResetEmailSubject");
            string body = L("PasswordResetEmailMessage", registrationLink);

            _emailService.SendAsync(args.Email, args.Email, subject, body).Wait();
        }
    }
}
