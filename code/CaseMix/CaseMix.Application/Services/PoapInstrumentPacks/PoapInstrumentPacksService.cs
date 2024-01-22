using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.PoapInstrumentPacks.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.PoapInstrumentPacks
{
    public class PoapInstrumentPacksService : CaseMixAppServiceBase, IPoapInstrumentPacksService
    {
        private readonly IRepository<InstrumentPack, int> _instrumentPackRepository;

        public PoapInstrumentPacksService(IRepository<InstrumentPack, int> instrumentPackRepository)
        {
            _instrumentPackRepository = instrumentPackRepository;
        }

        public async Task<IEnumerable<InstrumentPackDto>> GetAllInstrumentPacks()
        {
            var instrumentPacks = await _instrumentPackRepository.GetAll()
                .Select(e => ObjectMapper.Map<InstrumentPackDto>(e))
                .ToListAsync();

            return instrumentPacks;
        }
    }
}
