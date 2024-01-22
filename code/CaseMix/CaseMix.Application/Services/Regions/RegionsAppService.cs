using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using CaseMix.Entities;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.IntegratedCareSystems.Dto;
using CaseMix.Services.Regions.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.Regions
{
    public class RegionsAppService : CaseMixAppServiceBase, IRegionsAppService
    {
        private readonly IRepository<HospitalPatient, Guid> _hospitalPaientRepository;
        private readonly IRepository<UserHospital, Guid> _userHospitalsRepository;
        private readonly IRepository<Region, Guid> _RegionsRepository;
        private readonly IRepository<Hospital, string> _HospitalsRepository;
        private readonly IRepository<UserRealmMapping, Guid> _userRealmMappingsRepository;
        private readonly IRepository<IntegratedCareSystem, int> _icsRepository;
        private readonly IRepository<TrustIcsMapping, Guid> _trustIcsRepository;

        public RegionsAppService(
            IRepository<HospitalPatient, Guid> hospitalPaientRepository,
            IRepository<UserHospital, Guid> userHospitalsRepository,
            IRepository<Region, Guid> RegionsRepository,
            IRepository<UserRealmMapping, Guid> userRealmMappingsRepository,
            IRepository<Hospital, string> HospitalsRepository,
            IRepository<IntegratedCareSystem, int> icsRepository,
            IRepository<TrustIcsMapping, Guid> trustIcsRepository
            )
        {
            _hospitalPaientRepository = hospitalPaientRepository;
            _userHospitalsRepository = userHospitalsRepository;
            _RegionsRepository = RegionsRepository;
            _HospitalsRepository = HospitalsRepository;
            _userRealmMappingsRepository = userRealmMappingsRepository;
            _icsRepository = icsRepository;
            _trustIcsRepository = trustIcsRepository;

        }

        public async Task<IEnumerable<RegionDto>> GetAll(string countryName = null)
        {
            var regions = new List<RegionDto>();

            if (string.IsNullOrEmpty(countryName))
            {
                regions = await _RegionsRepository.GetAll()
                .OrderBy(e => e.Name)
                .Select(e => ObjectMapper.Map<RegionDto>(e))
                .ToListAsync();
            }
            else
            {
                var country = await _RegionsRepository.FirstOrDefaultAsync(e => e.Name == countryName);
                regions = await _RegionsRepository.GetAll()
                .Where(e => e.ParentId == country.Id)
                .OrderBy(e => e.Name)
                .Select(e => ObjectMapper.Map<RegionDto>(e))
                .ToListAsync();
            }

            return regions;
        }

        public async Task<bool> SaveUserRealm(RegionHospitalMappingDto input)
        {
            try
            {
                var regionIds = input.RegionIds.Select(e => e);
                var hospitalIds = input.HospitalIds.Select(e => e);

                var userRealms = await _userRealmMappingsRepository.GetAll().Where(e => e.UserId == input.UserId).ToListAsync();

                if (userRealms.Count > 0)
                    await _userRealmMappingsRepository.DeleteAsync(e => e.UserId == input.UserId);

                foreach (var r in regionIds)
                {
                    var data = new UserRealmMapping
                    {
                        Id = Guid.NewGuid(),
                        UserId = input.UserId,
                        RegionId = r,
                        HospitalId = null
                    };

                    await _userRealmMappingsRepository.InsertAsync(data);
                }

                foreach (var h in hospitalIds)
                {
                    var data = new UserRealmMapping
                    {
                        Id = Guid.NewGuid(),
                        UserId = input.UserId,
                        RegionId = null,
                        HospitalId = h
                    };

                    await _userRealmMappingsRepository.InsertAsync(data);
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CreateRegionDto> CreateAsync(CreateRegionDto input)
        {
            try
            {
                var name = await _RegionsRepository.FirstOrDefaultAsync(e => e.Name == input.Name && e.Type == input.Type);
                var id = Guid.NewGuid();
                if (name != null)
                {
                    //throw new UserFriendlyException("Region name already exists.");
                    name.ParentId = input.ParentId;
                    name.IsEnabled = true;

                    await _RegionsRepository.UpdateAsync(name);

                    return input;
                }

                var region = new Region
                {
                    Id = id,
                    Name = input.Name,
                    ParentId = input.ParentId,
                    IsEnabled = true,
                    Type = input.Type
                };

                if (input.Type == "Trust")
                {
                    foreach (var icsId in input.IcsIds)
                    {
                        await _trustIcsRepository.InsertAsync(new TrustIcsMapping
                        {
                            RegionId = id,
                            IntegratedCareSystemId = icsId
                        });
                    }
                }

                await _RegionsRepository.InsertAsync(region);

                return input;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CreateRegionDto> UpdateAsync(CreateRegionDto input)
        {
            try
            {
                var region = await _RegionsRepository.FirstOrDefaultAsync(e => e.Id == input.Id && e.Type == input.Type);

                region.Name = input.Name;

                await _RegionsRepository.UpdateAsync(region);

                if (input.Type == "Trust")
                {
                    await _trustIcsRepository.DeleteAsync(e => e.RegionId == input.Id);

                    foreach (var icsId in input.IcsIds)
                    {
                        await _trustIcsRepository.InsertAsync(new TrustIcsMapping
                        {
                            RegionId = input.Id,
                            IntegratedCareSystemId = icsId
                        });
                    }
                }


                return input;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DeleteResponseDto> DeleteAsync(string id, string type)
        {
            DeleteResponseDto result = new DeleteResponseDto();

            try
            {
                var ids = new List<string>();

                if (await AbleToDelete(id, type))
                {
                    if (type == "Trust")
                    {

                        var hospitals = await _HospitalsRepository.GetAll()
                         .Where(e => e.TrustId == Guid.Parse(id))
                         .ToListAsync();

                        foreach (var hospital in hospitals)
                        {
                            if (await AbleToDeleteHospital(hospital.Id))
                            {
                                var userMappings = await _userRealmMappingsRepository.GetAll()
                                    .Where(e => e.HospitalId == hospital.Id).ToListAsync();

                                if (userMappings != null)
                                {
                                    foreach (var um in userMappings)
                                    {
                                        await _userRealmMappingsRepository.DeleteAsync(um);
                                    }
                                }

                                await _HospitalsRepository.DeleteAsync(hospital);
                            }
                            else
                            {
                                result.Deleted = false;
                                result.ErrorMessage = "Unable to remove Sub-Region. Please remove the Hospitals and its Users.";

                                return result;
                            }
                        }

                        await _RegionsRepository.DeleteAsync(e => e.Id == Guid.Parse(id));
                        //await CurrentUnitOfWork.SaveChangesAsync();

                        result.Deleted = true;
                        result.SuccessMessage = "Sub Region Deleted Successfully.";

                    }
                    else
                    {
                        await _RegionsRepository.DeleteAsync(e => e.Id == Guid.Parse(id));
                        await CurrentUnitOfWork.SaveChangesAsync();

                        result.Deleted = true;
                        result.SuccessMessage = "Deleted Successfully.";
                    }
                }
                else
                {
                    if (type == "Country")
                    {
                        result.Deleted = false;
                        result.ErrorMessage = "Unable to remove country. Please remove the region.";
                    }
                    else if (type == "Region")
                    {
                        result.Deleted = false;
                        result.ErrorMessage = "Unable to remove region. Please remove the sub region and hospitals.";
                    }
                }
            }
            catch (UserFriendlyException ex)
            {
                result.Deleted = false;
                result.ErrorMessage = string.IsNullOrEmpty(ex.InnerException.Message) ? ex.Message : ex.InnerException.Message;
            }


            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<bool> AbleToDelete(string id, string type)
        {
            var query = await _RegionsRepository.GetAll()
                .Where(_ => _.ParentId == Guid.Parse(id)).ToListAsync();

            if (query != null)
            {
                if (query.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> AbleToDeleteHospital(string id)
        {
            var userHopitals = await _userHospitalsRepository.GetAll()
                               .Where(e => e.HospitalId == id).ToListAsync();
            if (userHopitals != null)
            {
                if (userHopitals.Count > 0)
                    return false;
                else
                {
                    var hospitalPatient = await _hospitalPaientRepository.GetAll()
                        .Where(e => e.HospitalId == id).ToListAsync();

                    if (hospitalPatient != null)
                    {
                        if (hospitalPatient.Count > 0)
                            return false;
                        else
                            return true;
                    }
                    else
                        return false;

                }
            }
            else
                return false;

        }

        public async Task<List<RegionManagementNodeDto>> GetAllRegionData(bool isRealm = false, long? userId = null)
        {
            var response = new List<RegionManagementNodeDto>();
            var ics = await _icsRepository.GetAll()
                .Select(e => ObjectMapper.Map<IntegratedCareSystemDto>(e))
                .ToListAsync();

            var parent = await _RegionsRepository.GetAll()
                .Where(e => e.ParentId == null)
                .ToListAsync();

            foreach (var p in parent)
            {
                var node = new RegionManagementNodeDto
                {
                    Key = p.Id.ToString(),
                    Label = p.Name,
                    Type = "person",
                    StyleClass = "p-person",
                    Expanded = true,
                    Data = new RegionManagementDataDto
                    {
                        Name = p.Name,
                        Avatar = null,
                        DataType = p.Type,
                        ParentId = p.ParentId.ToString()
                    }
                };

                var children = await GetChildrenCountries(ics, p.Id, isRealm, userId);

                if (children != null)
                {
                    var check = children.Where(e => e.Selected == false);

                    if (check.Count() > 0)
                        node.PartialSelected = true;
                    else
                        node.Selected = false;

                    node.Children = children;
                }


                response.Add(node);
            }

            return response;
        }

        public async Task<List<RegionManagementNodeDto>> GetChildrenCountries(List<IntegratedCareSystemDto> ics, Guid? parentId, bool isRealm = false, long? userId = null)
        {
            var response = new List<RegionManagementNodeDto>();

            var countries = await _RegionsRepository.GetAll()
                .Where(e => e.ParentId == parentId && e.Type == "Country")
                .ToListAsync();

            foreach (var c in countries)
            {
                var node = new RegionManagementNodeDto
                {
                    Key = c.Id.ToString(),
                    Label = c.Name,
                    Type = "person",
                    StyleClass = "p-person",
                    Expanded = true,
                    Data = new RegionManagementDataDto
                    {
                        Name = c.Name,
                        Avatar = null,
                        DataType = c.Type,
                        ParentId = c.ParentId.ToString()
                    },
                };

                var children = await GetChildrenRegions(ics, c.Id, isRealm, userId, c.Name);

                if (children != null)
                {
                    if (userId != null)
                    {
                        var userRealms = await _userRealmMappingsRepository.GetAll()
                                .Where(e => e.UserId == userId)
                                .Select(e => e.RegionId.ToString())
                                .ToListAsync();

                        var userRealmMapping = children
                            .Where(e => userRealms.Contains(e.Key));

                        if (userRealmMapping.Count() != 0 && children.Count() != 0 && userRealmMapping.Count() == children.Count())
                            node.Selected = true;
                        else if (userRealmMapping.Count() > 0)
                            node.PartialSelected = true;


                        foreach (var child in children)
                        {
                            if (child.PartialSelected.HasValue && child.PartialSelected == true && userRealmMapping.Count() == 0)
                            {
                                node.PartialSelected = true;
                                break;
                            }
                        }
                    }
                    node.Children = children;
                }



                response.Add(node);
            }

            if (countries.Count() < 7 && !isRealm)
            {
                var node = new RegionManagementNodeDto
                {
                    Key = "",
                    Label = " ",
                    Type = "person",
                    StyleClass = "p-person",
                    Expanded = true,
                    Icon = "https://img.icons8.com/emoji/2x/plus-emoji.png",
                    Data = new RegionManagementDataDto
                    {
                        DataType = "Country",
                        ParentId = parentId.ToString()
                    },
                };

                response.Add(node);
            }

            return response;
        }


        public async Task<List<RegionManagementNodeDto>> GetChildrenRegions(List<IntegratedCareSystemDto> ics, Guid? parentId, bool isRealm = false, long? userId = null, string countryName = "")
        {
            try
            {
                var response = new List<RegionManagementNodeDto>();

                var regions = await _RegionsRepository.GetAll()
                    .Where(e => e.ParentId == parentId && e.Type == "Region")
                    .ToListAsync();

                foreach (var r in regions)
                {
                    var node = new RegionManagementNodeDto
                    {
                        Key = r.Id.ToString(),
                        Label = r.Name,
                        Type = "person",
                        StyleClass = "p-person",
                        Expanded = true,
                        Data = new RegionManagementDataDto
                        {
                            Name = r.Name,
                            Avatar = null,
                            DataType = r.Type,
                            ParentId = r.ParentId.ToString(),
                            CountryName = countryName
                        },
                    };

                    var children = await GetChildrenTrusts(ics, r.Id, isRealm, userId, countryName, r.Name);

                    if (children != null)
                    {
                        if (userId != null)
                        {
                            var userRealms = await _userRealmMappingsRepository.GetAll()
                                    .Where(e => e.UserId == userId)
                                    .Select(e => e.HospitalId)
                                    .ToListAsync();

                            var userRealmMapping = children
                               .Where(e => userRealms.Contains(e.Key));

                            if (userRealmMapping.Count() == children.Count())
                                node.PartialSelected = false;
                            else if (userRealmMapping.Count() > 0)
                                node.PartialSelected = true;
                        }
                        node.Children = children;
                    }


                    response.Add(node);
                }

                if (regions.Count() < 7 && !isRealm)
                {
                    var node = new RegionManagementNodeDto
                    {
                        Key = "",
                        Label = " ",
                        Type = "person",
                        StyleClass = "p-person",
                        Expanded = true,
                        Icon = "https://img.icons8.com/emoji/2x/plus-emoji.png",
                        Data = new RegionManagementDataDto
                        {
                            DataType = "Region",
                            ParentId = parentId.ToString(),
                            CountryName = countryName
                        },
                    };

                    response.Add(node);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new List<RegionManagementNodeDto>();
            }

        }

        public async Task<List<RegionManagementNodeDto>> GetChildrenTrusts(List<IntegratedCareSystemDto> ics, Guid? parentId, bool isRealm = false, long? userId = null, string countryName = "", string regionName = "")
        {
            try
            {
                var response = new List<RegionManagementNodeDto>();

                var regions = await _RegionsRepository.GetAll()
                    .Where(e => e.ParentId == parentId && e.Type == "Trust")
                    .ToListAsync();

                foreach (var r in regions)
                {
                    var trustIcsmappings = await _trustIcsRepository.GetAll()
                        .Where(e => e.RegionId == r.Id)
                        .Select(e => e.IntegratedCareSystemId)
                        .ToListAsync();

                    var selectedIcs = ics.Where(e => trustIcsmappings.Any(f => f == e.Id)).ToList();
                    var node = new RegionManagementNodeDto
                    {
                        Key = r.Id.ToString(),
                        Label = r.Name,
                        Type = "person",
                        StyleClass = "p-person",
                        Expanded = true,
                        Data = new RegionManagementDataDto
                        {
                            Name = r.Name,
                            Avatar = null,
                            DataType = r.Type,
                            ParentId = r.ParentId.ToString(),
                            CountryName = countryName,
                            RegionName = regionName,
                            SelectedIcs = selectedIcs
                        },
                    };

                    var children = await GetChildrenHospitals(selectedIcs, r.Id, parentId, isRealm, countryName, regionName, r.Name);

                    if (children != null)
                    {
                        if (userId != null)
                        {
                            var userRealms = await _userRealmMappingsRepository.GetAll()
                                    .Where(e => e.UserId == userId)
                                    .Select(e => e.HospitalId)
                                    .ToListAsync();

                            var userRealmMapping = children
                               .Where(e => userRealms.Contains(e.Key));

                            if (userRealmMapping.Count() == children.Count())
                                node.PartialSelected = false;
                            else if (userRealmMapping.Count() > 0)
                                node.PartialSelected = true;
                        }
                        node.Children = children;
                    }
                    response.Add(node);
                }

                if (regions.Count() < 7 && !isRealm)
                {
                    var node = new RegionManagementNodeDto
                    {
                        Key = "",
                        Label = " ",
                        Type = "person",
                        StyleClass = "p-person",
                        Expanded = true,
                        Icon = "https://img.icons8.com/emoji/2x/plus-emoji.png",
                        Data = new RegionManagementDataDto
                        {
                            DataType = "Trust",
                            ParentId = parentId.ToString(),
                            RegionName = regionName,
                            CountryName = countryName
                        },
                    };

                    response.Add(node);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new List<RegionManagementNodeDto>();
            }

        }

        public async Task<List<RegionManagementNodeDto>> GetChildrenHospitals(List<IntegratedCareSystemDto> ics, Guid? regionId, Guid? countryId = null, bool isRealm = false, string countryName = "", string regionName = "", string trustName = "")
        {
            try
            {
                var response = new List<RegionManagementNodeDto>();

                var hospitals = await _HospitalsRepository.GetAll()
                    .Where(e => e.TrustId == regionId)
                    .ToListAsync();

                foreach (var h in hospitals)
                {
                    var node = new RegionManagementNodeDto
                    {
                        Key = h.Id,
                        Label = h.Name,
                        Type = "person",
                        StyleClass = "p-person",
                        Expanded = true,
                        Data = new RegionManagementDataDto
                        {
                            Name = h.Name,
                            Avatar = null,
                            DataType = "Hospital",
                            Postcode = h.Postcode,
                            RegionName = regionName,
                            CountryName = countryName,
                            TrustName = trustName,
                            SelectedIcs = ics,
                            activeDevMgt = h.ActiveDevMgt,
                            ShowButtonDevProc = h.ShowButtonDevProc?? true                            
                        },
                    };

                    response.Add(node);
                };

                if (hospitals.Count() < 7 && !isRealm)
                {
                    var node = new RegionManagementNodeDto
                    {
                        Key = "",
                        Label = " ",
                        Type = "person",
                        StyleClass = "p-person",
                        Expanded = true,
                        Icon = "https://img.icons8.com/emoji/2x/plus-emoji.png",
                        Data = new RegionManagementDataDto
                        {
                            DataType = "Hospital",
                            ParentId = regionId.ToString(),
                            RegionName = regionName,
                            CountryName = countryName,
                            TrustName = trustName,
                            SelectedIcs = ics
                        },
                    };

                    response.Add(node);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new List<RegionManagementNodeDto>();
            }
        }

        public async Task<IEnumerable<RegionManagementNodeDto>> GetAllRegionsMultiSelect(long? userId = null)
        {

            var regions = new List<RegionDto>();
            var data = new List<RegionManagementNodeDto>();
            try
            {
                var userRealms = await _userRealmMappingsRepository.GetAll()
                .Where(e => e.UserId == userId)
                .Select(e => e.RegionId)
                .ToListAsync();

                if (userId != null)
                {
                    regions = await _RegionsRepository.GetAll()
                    .Where(e => e.Type == "Region" && userRealms.Contains(e.Id))
                    .Select(e => ObjectMapper.Map<RegionDto>(e))
                    .ToListAsync();
                }
                else
                {
                    regions = await _RegionsRepository.GetAll()
                    .Where(e => e.Type == "Region")
                    .Select(e => ObjectMapper.Map<RegionDto>(e))
                    .ToListAsync();
                }

                data = regions.Select(r => new RegionManagementNodeDto
                {
                    Key = r.Id.ToString(),
                    Type = "person",
                    StyleClass = "p-person",
                    Label = r.Name,
                    Expanded = true,
                    Data = new RegionManagementDataDto
                    {
                        Name = r.Name,
                        DataType = "Region"
                    }
                }).ToList();

                return data;
            }

            catch (Exception ex)
            {
                return data;
            }
        }
    }
}
