using Abp.Authorization;
using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.UkRegions.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.UkRegions
{
    public class UkRegions : CaseMixAppServiceBase, IUkRegionsAppService
    {
        private readonly IRepository<UkRegion, int> _UkRegionsRepository;
        private readonly IRepository<Region, Guid> _RegionsRepository;

        public UkRegions(
             IRepository<UkRegion, int> UkRegionsRepository,
             IRepository<Region, Guid> RegionsRepository
            )
        {
            _UkRegionsRepository = UkRegionsRepository;
            _RegionsRepository = RegionsRepository;
        }

        public async Task<IEnumerable<UkRegionDto>> GetAll()
        {
            try
            {
                var country = await _RegionsRepository.FirstOrDefaultAsync(e => e.Name == "United Kingdom");
                var regions = await _RegionsRepository.GetAll()
                    .Where(e => e.ParentId == country.Id)
                    .Select(e => e.Name)
                    .ToListAsync();

                var ukRegions = await _UkRegionsRepository.GetAll()
                      .Where(e => !regions.Any(r => r == e.Name))
                      .OrderBy(e => e.Name)
                      .Select(e => ObjectMapper.Map<UkRegionDto>(e))
                      .ToListAsync();

                return ukRegions;
            }
            catch(Exception ex)
            {
                return new List<UkRegionDto>();
            }
            
        }
    }
}
