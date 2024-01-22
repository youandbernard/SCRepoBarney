using Abp.Application.Services;
using CaseMix.MultiTenancy.Dto;

namespace CaseMix.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

