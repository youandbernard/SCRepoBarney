using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.PoapRiskFactors.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.PoapRiskFactors
{
    public class PoapRiskFactorAppService : CaseMixAppServiceBase, IPoapRiskFactorAppService
    {
        private readonly IRepository<PoapRiskFactor, int> _poapRiskFactorRepository;

        public PoapRiskFactorAppService(IRepository<PoapRiskFactor, int> poapRiskFactorRepository)
        {
            _poapRiskFactorRepository = poapRiskFactorRepository;
        }

        public async Task<IEnumerable<PoapRiskFactorDto>> GetAllAsync()
        {
            var result = await _poapRiskFactorRepository.GetAll()
                .Select(e => ObjectMapper.Map<PoapRiskFactorDto>(e))
                .ToListAsync();

            return result;
        }
    }
}
