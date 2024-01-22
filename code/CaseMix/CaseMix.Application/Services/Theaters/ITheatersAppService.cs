using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using CaseMix.Dto.Output;
using CaseMix.Services.Theaters.Dto;

namespace CaseMix.Services.Theaters
{
    public interface ITheatersAppService : IAsyncCrudAppService<TheaterDto, Guid, PagedTheaterResultRequestDto, OutputDto, TheaterDto>
    {
        Task<OutputDto> CreateTheater(TheaterDto input);
        Task<IEnumerable<SearchTheaterDto>> Search(string hospitalId, string keyword);
        Task<IEnumerable<TheaterDto>> GetByHospitalId(string hospitalId);
        Task<IEnumerable<SearchTheaterDto>> GetAllTheaters(string hospitalId);
    }
}
