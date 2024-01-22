using Abp.Authorization;
using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.Manufactures.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.Manufactures
{
    public class Manufactures : CaseMixAppServiceBase, IManufacturesAppService
    {
        private readonly IRepository<Manufacture, Guid> _ManufacturesRepository;

        public Manufactures(
             IRepository<Manufacture, Guid> ManufacturesRepository
            )
        {
            _ManufacturesRepository = ManufacturesRepository;
        }

        public async Task<IEnumerable<ManufactureDto>> GetAll()
        {
            try
            {
                var manufactures = await _ManufacturesRepository.GetAll()
               .OrderBy(e => e.Name)
               .Select(e => ObjectMapper.Map<ManufactureDto>(e))
               .ToListAsync();

                return manufactures;
            }
            catch (Exception ex)
            {
                return new List<ManufactureDto>();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        public async Task<ManufactureDto> GetById(Guid manufacturerId)
        {
            try
            {
                var manufacturer = await _ManufacturesRepository.GetAll()
                    .Where(_ => _.Id == manufacturerId)
                   .OrderBy(e => e.Name)
                   .Select(e => ObjectMapper.Map<ManufactureDto>(e))
                   .FirstOrDefaultAsync();

                return manufacturer;
            }
            catch (Exception ex)
            {
                return new ManufactureDto();
            }

        }
    }
}
