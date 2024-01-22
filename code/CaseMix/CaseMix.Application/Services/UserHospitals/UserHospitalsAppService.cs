using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.UserHospitals.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.UserHospitals
{
    public class UserHospitalsAppService : CaseMixAppServiceBase, IUserHospitalsAppService
    {
        private readonly IRepository<UserHospital, Guid> _userHospitalsRepository;
        private readonly IRepository<Hospital, string> _hospitalRepository;

        public UserHospitalsAppService(IRepository<UserHospital, Guid> userHospitalsRepository, IRepository<Hospital, string> hospitalRepository)
        {
            _userHospitalsRepository = userHospitalsRepository;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<IEnumerable<UserHospitalDto>> GetAll(long userId)
        {
            var userHospitals = await _userHospitalsRepository.GetAll()
                .Include(e => e.Hospital)
                .Where(e => e.UserId == userId)
                .Select(e => ObjectMapper.Map<UserHospitalDto>(e))
                .ToListAsync();

            userHospitals.ForEach(_ => _.IsSelected = true);

            return userHospitals;
        }

        public async Task SaveAll(IEnumerable<UserHospitalDto> inputs)
        {
            await _userHospitalsRepository.DeleteAsync(e => e.UserId == inputs.FirstOrDefault().UserId);
            inputs = inputs.Where(e => e.IsSelected);
            foreach (var input in inputs)
            {
                var userHospital = ObjectMapper.Map<UserHospital>(input);
                await _userHospitalsRepository.InsertOrUpdateAsync(userHospital);
            }
        }
    }
}
