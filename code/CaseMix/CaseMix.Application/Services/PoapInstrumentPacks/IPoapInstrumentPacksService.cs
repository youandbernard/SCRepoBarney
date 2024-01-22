using Abp.Application.Services;
using CaseMix.Services.PoapInstrumentPacks.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.PoapInstrumentPacks
{
    public interface IPoapInstrumentPacksService: IApplicationService
    {
        Task<IEnumerable<InstrumentPackDto>> GetAllInstrumentPacks();
    }
}
