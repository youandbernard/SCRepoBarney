using Abp.Domain.Repositories;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.PoapProcedureDevices.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.PoapProcedureDevices
{
    public class PoapProcedureDevicesAppService : CaseMixAppServiceBase, IPoapProcedureDevicesAppService
    {
        private readonly IRepository<CaseMix.Entities.PoapProcedureDevices, int> _devicePoapProcDevRepository;
        public PoapProcedureDevicesAppService(IRepository<CaseMix.Entities.PoapProcedureDevices, int> devicePoapProcDevRepository)
        {
            _devicePoapProcDevRepository = devicePoapProcDevRepository;
        }
        public async Task<IEnumerable<PoapProcedureDevicesDto>> GetByProcedureId(string poapProcedureId, string poapId)
        {
            var query = await _devicePoapProcDevRepository.GetAll()
                .Include(_ => _.Device)
                .Where(_ => _.PoapProcedureId == new Guid(poapProcedureId)
                    && _.PreOperativeAssessmentId == new Guid(poapId))
                .ToListAsync();

            IList<PoapProcedureDevicesDto> poapProcedureDevicesDto = new List<PoapProcedureDevicesDto>();

            if (query != null && query.Count() > 0)
            {
                query.ForEach(
                    _ => poapProcedureDevicesDto.Add(
                        new PoapProcedureDevicesDto
                        {
                            DeviceId = _.DeviceId,
                            CreatedDate = _.CreatedDate,
                            Id = _.Id,
                            ModifiedBy = _.ModifiedBy,
                            ModifiedDate = _.ModifiedDate,
                            PoapProcedureId = _.PoapProcedureId,
                            PreOperativeAssessmentId = _.PreOperativeAssessmentId,
                            SnomedId = _.SnomedId,
                            UserId = _.UserId,
                            Device = new DeviceDto
                            {
                                Id = _.Device.Id,
                                DeviceName = _.Device.DeviceName,
                                DeviceDescription = _.Device.DeviceDescription,
                                BrandName = _.Device.BrandName,
                                UID = _.Device.UID,
                                GMDNTermCode = _.Device.GMDNTermCode,
                                Model = _.Device.Model,
                                DocFileId = _.Device.DocFileId,
                                Status = _.Device.Status,
                                UserId = _.Device.UserId,
                                CreatedDate = _.Device.CreatedDate,
                                ManufacturerId = _.Device.ManufacturerId,
                                ModifiedDate = _.Device.ModifiedDate
                            }
                        })
                );
            }

            return poapProcedureDevicesDto;
        }

        public async Task SavePoapProcedureDevices(IEnumerable<DeviceDto> deviceDtos, Guid poapId, Guid poapProcedureId, string snomedId)
        {
            await _devicePoapProcDevRepository.DeleteAsync(e =>
                               e.PreOperativeAssessmentId == poapId &&
                               e.PoapProcedureId == poapProcedureId);

            IList<CaseMix.Entities.PoapProcedureDevices> poapProcedureDevices
                = new List<CaseMix.Entities.PoapProcedureDevices>();

            deviceDtos.ToList().ForEach(_ =>
                poapProcedureDevices.Add(new Entities.PoapProcedureDevices
                {
                    DeviceId = _.Id,
                    PoapProcedureId = poapProcedureId,
                    PreOperativeAssessmentId = poapId,
                    SnomedId = snomedId,
                    UserId = AbpSession.UserId.Value,
                    CreatedDate = DateTime.UtcNow
                })
            );

            IList<Entities.PoapProcedureDevices> existing = new List<Entities.PoapProcedureDevices>();

            if (poapProcedureDevices.Count() > 0)
            {
                foreach (var _ in poapProcedureDevices)
                {
                    var exists = existing.Any(e => e.PoapProcedureId == _.PoapProcedureId &&
                            e.DeviceId == _.DeviceId &&
                            e.PreOperativeAssessmentId == _.PreOperativeAssessmentId);

                    if (!exists)
                        await _devicePoapProcDevRepository.InsertAsync(_);
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            // await CheckIfNotSelectedButExisted(poapProcedureDevices, existing);
        }

        private async Task CheckIfNotSelectedButExisted(IList<Entities.PoapProcedureDevices> poapProcedureDevices,
            IList<Entities.PoapProcedureDevices> existingPoapProcedureDevices)
        {
            var query = existingPoapProcedureDevices.Where(_ => !poapProcedureDevices.Contains(_)).ToList();

            if (query != null)
            {
                if (query.Count() > 0)
                {
                    foreach (var _ in query)
                    {
                        await _devicePoapProcDevRepository.DeleteAsync(_);
                    }

                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}
