using Abp.Application.Services;
using CaseMix.Services.PasswordResets.Dto;
using System;
using System.Threading.Tasks;

namespace CaseMix.Services.PasswordResets
{
    public interface IPasswordResetsAppService : IApplicationService
    {
        Task Create(string emailAddress);
        Task Validate(Guid id);
        Task ResetPassword(PasswordResetInputDto input);
    }
}
