using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.Ethnicities.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.Ethnicities
{
    public class EthnicitiesAppService : CaseMixAppServiceBase, IEthnicitiesAppService
    {
        private readonly IRepository<Ethnicity> _ethnicitiesRepository;

        public EthnicitiesAppService(IRepository<Ethnicity> ethnicitiesRepository)
        {
            this._ethnicitiesRepository = ethnicitiesRepository;
        }

        public async Task<IEnumerable<EthnicityDto>> GetAll()
        {
            var ethnicities = await _ethnicitiesRepository.GetAll()
                .OrderBy(e => e.Description)
                .Select(e => ObjectMapper.Map<EthnicityDto>(e))
                .ToListAsync();
            return ethnicities;
        }
    }
}
