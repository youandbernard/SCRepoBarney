using Abp.Authorization.Users;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CaseMix.Services.PasswordResets.Dto
{
    public class PasswordResetInputDto : IValidatableObject
    {
        public Guid Id { get; set; }
        
        [Required]
        [Description("New Password")]
        [StringLength(AbpUserBase.MaxPlainPasswordLength, MinimumLength = CaseMixConsts.PasswordMinLength)]
        public string NewPassword { get; set; }
        
        [Required]
        [Description("New Password Confirmation")]
        public string NewPasswordConfirmation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NewPassword.IsNullOrEmpty() && !NewPasswordConfirmation.IsNullOrEmpty())
            {
                if (!NewPassword.Equals(NewPasswordConfirmation))
                {
                    yield return new ValidationResult("Password and Confirmation does not match.");
                }
                if (!(Regex.Match(NewPassword, CaseMixConsts.PasswordRegexValidator)).Success)
                {
                    yield return new ValidationResult(CaseMixConsts.InvalidPasswordValidationMessage);
                }
            }
        }
    }
}
