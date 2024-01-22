using CaseMix.DomainServices;
using CaseMix.Services.PasswordResets.Dto;
using System;
using System.Threading.Tasks;

namespace CaseMix.Services.PasswordResets
{
    public class PasswordResetsAppService : CaseMixAppServiceBase, IPasswordResetsAppService
    {
        private readonly IPasswordResetsDomainService _passwordResetsDomainService;

        public PasswordResetsAppService(IPasswordResetsDomainService passwordResetsDomainService)
        {
            _passwordResetsDomainService = passwordResetsDomainService;
        }

        public async Task Create(string emailAddress)
        {
            await _passwordResetsDomainService.InsertAsync(emailAddress);
        }

        public async Task Validate(Guid id)
        {
            await _passwordResetsDomainService.ValidateAsync(id);
        }

        public async Task ResetPassword(PasswordResetInputDto input)
        {
            await _passwordResetsDomainService.ResetPasswordAsync(input.Id, input.NewPassword);
        }
    }
}
