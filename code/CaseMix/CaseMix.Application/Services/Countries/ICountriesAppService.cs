using Abp.Application.Services;
using CaseMix.Services.Countries.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.Countries
{
    public interface ICountriesAppService : IApplicationService
    {
        Task<IEnumerable<CountryDto>> GetAll(bool? all);
    }
}
