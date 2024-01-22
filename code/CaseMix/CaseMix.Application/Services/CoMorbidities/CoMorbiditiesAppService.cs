using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.CoMorbidities.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.CoMorbidities
{
    //@TODO: Move this to DB when the items are finalized
    public class CoMorbiditiesAppService : CaseMixAppServiceBase, ICoMorbiditiesAppService
    {
        private readonly IRepository<ComorbidityGroup, int> _comorbidityGroupRepository;
        private readonly IRepository<Comorbidity, int> _comorbididtyRepository;

        public CoMorbiditiesAppService(
            IRepository<ComorbidityGroup, int> comorbidityGroupRepository,
            IRepository<Comorbidity, int> comorbidityRepository
        )
        {
            _comorbidityGroupRepository = comorbidityGroupRepository;
            _comorbididtyRepository = comorbidityRepository;
        }

        public async Task<IEnumerable<CoMorbidityGroupDto>> GetAll()
        {
            var comorbidities = await _comorbidityGroupRepository.GetAll()
            .Include(e => e.Comorbidities)
            .Select(e => ObjectMapper.Map<CoMorbidityGroupDto>(e))
            .ToListAsync();

            return comorbidities;
        }
    }
}
