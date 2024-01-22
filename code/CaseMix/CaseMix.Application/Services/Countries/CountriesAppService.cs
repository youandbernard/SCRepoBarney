using Abp.Authorization;
using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.Countries.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.Countries
{
    public class Countries : CaseMixAppServiceBase, ICountriesAppService
    {
        private readonly IRepository<Country, int> _CountriesRepository;
        private readonly IRepository<Region, Guid> _RegionsRepository;

        public Countries(
             IRepository<Country, int> CountriesRepository,
             IRepository<Region, Guid> RegionsRepository
            )
        {
            _CountriesRepository = CountriesRepository;
            _RegionsRepository = RegionsRepository;
        }

        public async Task<IEnumerable<CountryDto>> GetAll(bool? all)
        {

            var regions = await _RegionsRepository.GetAll()
                  .Where(e => e.Type == "Country")
                  .Select(e => e.Name)
                  .ToListAsync();

            var countries = new List<CountryDto>();

            if(all.HasValue && all.Value)
                countries = await _CountriesRepository.GetAll()
                    .Where(e => !regions.Any(r => r == e.Name))
                    .OrderBy(e => e.Name)
                    .Select(e => ObjectMapper.Map<CountryDto>(e))
                    .ToListAsync();
            else
                countries = await _CountriesRepository.GetAll()
                   .Where(e => regions.Any(r => r == e.Name))
                   .OrderBy(e => e.Name)
                   .Select(e => ObjectMapper.Map<CountryDto>(e))
                   .ToListAsync();

            return countries;
        }
    }
}
