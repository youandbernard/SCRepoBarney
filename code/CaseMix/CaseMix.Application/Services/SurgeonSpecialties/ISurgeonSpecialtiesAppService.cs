using Abp.Application.Services;
using CaseMix.Services.UserSpecialties.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.SurgeonSpecialties
{
    public interface ISurgeonSpecialtiesAppService: IApplicationService
    {
        Task<IEnumerable<SurgeonSpecialtyDto>> GetAll(long userId);
        Task SaveAll(IEnumerable<SurgeonSpecialtyDto> inputs);
    }
}
