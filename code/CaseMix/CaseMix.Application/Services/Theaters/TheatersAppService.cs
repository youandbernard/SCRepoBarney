using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using CaseMix.Dto.Output;
using CaseMix.Entities;
using CaseMix.Services.Theaters.Dto;
using Microsoft.EntityFrameworkCore;

namespace CaseMix.Services.Theaters
{
    public class TheatersAppService : AsyncCrudAppService<Theater, TheaterDto, Guid, PagedTheaterResultRequestDto, OutputDto, TheaterDto>, ITheatersAppService
    {
        public TheatersAppService(IRepository<Theater, Guid> theatersRepository) : base (theatersRepository)
        {
        }

        protected override IQueryable<Theater> CreateFilteredQuery(PagedTheaterResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                .Where(e => e.HospitalId == input.HospitalId)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), e => e.TheaterId.ToLower().Contains(input.Keyword.ToLower())
                    || e.Name.ToLower().Contains(input.Keyword.ToLower()));
        }

        public async Task<IEnumerable<SearchTheaterDto>> Search(string hospitalId, string keyword)
        {
            var theaters = await Repository.GetAll()
                .Where(e => e.HospitalId == hospitalId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), e => e.TheaterId.ToLower().Contains(keyword.ToLower())
                    || e.Name.ToLower().Contains(keyword.ToLower()))
                .OrderBy(e => e.TheaterId)
                .Take(10)
                .Select(e => ObjectMapper.Map<SearchTheaterDto>(e))
                .ToListAsync();

            return theaters;
        }

        public async Task<IEnumerable<SearchTheaterDto>> GetAllTheaters(string hospitalId)
        {
            var theaters = await Repository.GetAll()
                .Where(e => e.HospitalId == hospitalId)
                .OrderBy(e => e.TheaterId)
                .Select(e => ObjectMapper.Map<SearchTheaterDto>(e))
                .ToListAsync();

            return theaters;
        }

        public async Task<IEnumerable<TheaterDto>> GetByHospitalId(string hospitalId)
        {
           return await Repository.GetAll()
                .Where(e => e.HospitalId == hospitalId)
                .OrderBy(e => e.TheaterId)
                .Select(e => ObjectMapper.Map<TheaterDto>(e))
                .ToListAsync();
        }

        public async Task<OutputDto> CreateTheater(TheaterDto input) 
        {
            OutputDto output = new OutputDto(); 
            try
            {
                var name = await Repository.FirstOrDefaultAsync(e => e.Name == input.Name && e.HospitalId == input.HospitalId);
                var theaterId = await Repository.FirstOrDefaultAsync(e => e.TheaterId == input.TheaterId && e.HospitalId == input.HospitalId);

                if (name != null)
                {
                    output.IsError = true;
                    output.IsSuccess = false;
                    output.ErrorMessage = "Theater name already exists from current hospital.";

                    return output;
                }
                if (theaterId != null)
                {
                    output.IsError = true;
                    output.IsSuccess = false;
                    output.ErrorMessage = "Theater ID already exists from current hospital.";

                    return output;
                }

                var theater = new Theater
                {
                    TheaterId = input.TheaterId,
                    Name = input.Name,
                    HospitalId = input.HospitalId
                };

                await Repository.InsertAsync(theater);

                output.IsError = false;
                output.IsSuccess = true;

            }
            catch(Exception ex)
            {
                output.IsError = true;
                output.IsSuccess = false;
                output.ErrorMessage = ex.Message;
            }

            return output;
            //return input;
        }
    }
}