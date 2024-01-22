using Abp.Authorization;
using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.TrustIcsMappings.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.TrustIcsMappings
{
    public class TrustIcsMappings : CaseMixAppServiceBase, ITrustIcsMappingsAppService
    {
        private readonly IRepository<TrustIcsMapping, Guid> _TrustIcsMappingsRepository;

        public TrustIcsMappings(
             IRepository<TrustIcsMapping, Guid> TrustIcsMappingsRepository
            )
        {
            _TrustIcsMappingsRepository = TrustIcsMappingsRepository;
        }

        public async Task<IEnumerable<TrustIcsMappingDto>> GetAll()
        {

            var trustIcsMappings = await _TrustIcsMappingsRepository.GetAll()
                  .Select(e => ObjectMapper.Map<TrustIcsMappingDto>(e))
                  .ToListAsync();

            return trustIcsMappings;
        }
    }
}
