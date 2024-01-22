using Abp.Authorization;
using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.IntegratedCareSystems.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.IntegratedCareSystems
{
    public class IntegratedCareSystems : CaseMixAppServiceBase, IIntegratedCareSystemsAppService
    {
        private readonly IRepository<IntegratedCareSystem, int> _integratedCareSystemsRepository;

        public IntegratedCareSystems(
             IRepository<IntegratedCareSystem, int> integratedCareSystemsRepository
            )
        {
            _integratedCareSystemsRepository = integratedCareSystemsRepository;
        }

        public async Task<IEnumerable<IntegratedCareSystemDto>> GetAll()
        {

            var ics = await _integratedCareSystemsRepository.GetAll()
                  .Select(e => ObjectMapper.Map<IntegratedCareSystemDto>(e))
                  .ToListAsync();

            return ics;
        }
    }
}
