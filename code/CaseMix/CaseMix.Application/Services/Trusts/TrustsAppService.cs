using Abp.Authorization;
using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.Regions.Dto;
using CaseMix.Services.Trusts.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.Trusts
{
    public class Trusts : CaseMixAppServiceBase, ITrustsAppService
    {
        private readonly IRepository<TrustEntity, int> _TrustsRepository;
        private readonly IRepository<Region, Guid> _regionRepository;

        public Trusts(
             IRepository<TrustEntity, int> TrustsRepository,
             IRepository<Region, Guid> regionRepository
            )
        {
            _TrustsRepository = TrustsRepository;
            _regionRepository = regionRepository;
        }

        public async Task<IEnumerable<TrustsDto>> GetAll(Guid? RegionId = null, string regionName = null, bool fromHospitalManagement = false, string countryName = "")
        {
            var trustList = new List<TrustsDto>();

            if (countryName != null)
            {
                if (countryName.ToLower() == "united kingdom")
                {
                    var list = new List<TrustDto>();

                    var trustRegionIds = await _regionRepository.GetAll()
                    .Where(e => e.ParentId == RegionId)
                    .Select(e => e.Name)
                    .ToListAsync();

                    if (fromHospitalManagement)
                    {
                        list = await _TrustsRepository.GetAll()
                        .Where(e => !trustRegionIds.Any(f => f == e.GroupTrust))
                        .Select(e => ObjectMapper.Map<TrustDto>(e))
                        .ToListAsync();
                    }
                    else
                    {
                        list = await _TrustsRepository.GetAll()
                        .Where(e => !trustRegionIds.Any(f => f == e.GroupTrust))
                        .Select(e => ObjectMapper.Map<TrustDto>(e))
                        .ToListAsync();
                    }

                    if (regionName != null)
                        list = list.Where(e => e.Region.Contains(regionName)).ToList();

                    if (list.Count > 0)
                    {
                        list.ForEach(_ => trustList.Add(new TrustsDto
                        {
                            Type = "Trust",
                            GroupTrust = _.GroupTrust,
                            IsEnabled = true,
                            ParentId = null,
                            RegionName = _.Region,
                            RegionId = null,
                            TrustId = _.Id
                        }));
                    }
                }
                else
                {
                    var regions = new List<RegionDto>();

                    var region = await _regionRepository.FirstOrDefaultAsync(e => e.Name == regionName);
                    regions = await _regionRepository.GetAll()
                    .Where(e => e.ParentId == region.Id)
                    .OrderBy(e => e.Name)
                    .Select(e => ObjectMapper.Map<RegionDto>(e))
                    .ToListAsync();

                    if (regions.Count > 0)
                    {
                        regions.ForEach(_ => trustList.Add(new TrustsDto
                        {
                            Type = _.Type,
                            GroupTrust = _.Name,
                            IsEnabled = _.IsEnabled,
                            ParentId = _.ParentId,
                            RegionName = regionName,
                            RegionId = _.Id,
                            TrustId = null
                        }));
                    }
                }
            }

            return trustList;
        }

    }
}
