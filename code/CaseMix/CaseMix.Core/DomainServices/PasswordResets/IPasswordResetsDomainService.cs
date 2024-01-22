using Abp.Domain.Services;
using System;
using System.Threading.Tasks;

namespace CaseMix.DomainServices
{
    public interface IPasswordResetsDomainService : IDomainService
    {
        Task InsertAsync(string email);
        Task ValidateAsync(Guid id);
        Task ResetPasswordAsync(Guid id, string password);
    }
}
