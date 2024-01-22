using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.UserSpecialties.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CaseMix.Services.SurgeonSpecialties
{
    public class SurgeonSpecialtiesAppService : CaseMixAppServiceBase, ISurgeonSpecialtiesAppService
    {
        private readonly IRepository<SurgeonSpecialty, int> _surgeonSpecialtyRepository;
        public SurgeonSpecialtiesAppService(IRepository<SurgeonSpecialty, int> surgeonSpecialtyRepository)
        {
            _surgeonSpecialtyRepository = surgeonSpecialtyRepository;
        }

        public async Task<IEnumerable<SurgeonSpecialtyDto>> GetAll(long userId)
        {
            var specialties = await _surgeonSpecialtyRepository.GetAll()
                .Where(e => e.SurgeonId == userId)
                .Select(e => ObjectMapper.Map<SurgeonSpecialtyDto>(e))
                .ToListAsync();

            return specialties;
        }

        public async Task SaveAll(IEnumerable<SurgeonSpecialtyDto> inputs)
        {
            await _surgeonSpecialtyRepository.DeleteAsync(e => e.SurgeonId == inputs.FirstOrDefault().SurgeonId);
            inputs = inputs.Where(e => e.IsSelected);
            foreach(var input in inputs)
            {
                var surgeonSpecialty = ObjectMapper.Map<SurgeonSpecialty>(input);
                await _surgeonSpecialtyRepository.InsertAsync(surgeonSpecialty);
            }
        }
    }
}
