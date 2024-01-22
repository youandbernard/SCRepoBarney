using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using CaseMix.Entities;
using CaseMix.Services.Dashboard;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Regions.Dto;
using CaseMix.Services.UserHospitals.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CaseMix.Services.Hospitals
{
    public class HospitalsAppService : AsyncCrudAppService<Hospital, HospitalDto, string, PagedHospitalResultRequestDto>, IHospitalsAppService
    {
        private readonly IDashboardAppService _dashboardAppService;

        private readonly IRepository<UserHospital, Guid> _userHospitalsRepository;
        private readonly IRepository<Region, Guid> _RegionsRepository;
        private readonly IRepository<UserRealmMapping, Guid> _userRealmRepository;
        private readonly IRepository<SpecialtyInfo, int> _specialtyInfoRepository;

        private readonly UserManager _userManager;

        public HospitalsAppService(
            IDashboardAppService dashboardAppService,
            UserManager userManager,
            IRepository<SpecialtyInfo, int> specialtyInfoRepository,
            IRepository<UserHospital, Guid> userHospitalsRepository,
            IRepository<Hospital, string> hospitalsRepository,
            IRepository<UserRealmMapping, Guid> userRealmRepository,
            IRepository<Region, Guid> regionsRepository) : base(hospitalsRepository)
        {
            _dashboardAppService = dashboardAppService;
            _userHospitalsRepository = userHospitalsRepository;
            _RegionsRepository = regionsRepository;
            _userRealmRepository = userRealmRepository;
            _specialtyInfoRepository = specialtyInfoRepository;
            _userManager = userManager;
        }

        protected override IQueryable<Hospital> CreateFilteredQuery(PagedHospitalResultRequestDto input)
        {
            try
            {
                var hospitals = base.CreateFilteredQuery(input)
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), e => e.Id.ToLower().Contains(input.Keyword.ToLower())
                   || e.Name.ToLower().Contains(input.Keyword.ToLower()));

                return hospitals;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<IEnumerable<HospitalDto>> GetByUser(long id)
        {
            try
            {
                var hospitals = await _userHospitalsRepository.GetAll()
                .Include(e => e.Hospital)
                    .ThenInclude(_ => _.Setting)
                .Where(e => e.UserId == id)
                .OrderBy(e => e.Hospital.Name)
                .Select(e => ObjectMapper.Map<HospitalDto>(e.Hospital))
                .ToListAsync();

                return hospitals;
            }
            catch (Exception ex)
            {
                return new List<HospitalDto>();
            }
        }

        public async Task<HospitalDto> GetByHospitalId(string id)
        {
            try
            {
                var hospital = await Repository.GetAll()
                .Where(e => e.Id == id)
                .Select(e => ObjectMapper.Map<HospitalDto>(e))
                .FirstOrDefaultAsync();

                if (hospital.TrustId != null)
                {
                    var trust = await _RegionsRepository.FirstOrDefaultAsync(e => e.Id == hospital.TrustId);
                    if (trust != null)
                    {
                        var region = await _RegionsRepository.FirstOrDefaultAsync(e => e.Id == trust.ParentId);
                        if (region != null)
                        {
                            var country = await _RegionsRepository.FirstOrDefaultAsync(e => e.Id == region.ParentId);
                            hospital.RegionName = region.Name;
                            hospital.CountryName = country.Name;
                            hospital.TrustName = trust.Name;
                        }
                    }
                }

                return hospital;
            }
            catch (Exception Ex)
            {
                return new HospitalDto();
            }

        }

        public override async Task<HospitalDto> CreateAsync(HospitalDto input)
        {
            try
            {
                if (input.TrustName != null)
                {
                    var trust = await _RegionsRepository.FirstOrDefaultAsync(e => e.Name == input.TrustName);
                    var region = await _RegionsRepository.FirstOrDefaultAsync(e => e.Name == input.RegionName);
                    var id = Guid.NewGuid();
                    if (trust == null)
                    {
                        var newTrust = new Region
                        {
                            Id = id,
                            Name = input.TrustName,
                            Type = "Trust",
                            ParentId = region.Id,
                            IsEnabled = true
                        };

                        await _RegionsRepository.InsertAsync(newTrust);
                    }

                    var hospital = new Hospital
                    {
                        Name = input.Name,
                        Id = input.Id,
                        TrustId = trust == null ? id : trust.Id,
                        Postcode = input.Postcode,
                        ActiveDevMgt = input.ActiveDevMgt,
                        ShowButtonDevProc = input.ShowButtonDevProc                        
                    };

                    await Repository.InsertAsync(hospital);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    await SaveUserHospital(hospital, region);

                    await _dashboardAppService.UploadHospitalAsync(hospital);

                    if (!input.ActiveDevMgt)
                    {
                        await _specialtyInfoRepository.DeleteAsync(e => e.HospitalId == hospital.Id);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }
                else
                {
                    var name = await Repository.FirstOrDefaultAsync(e => e.Name == input.Name);
                    var hospitalId = await Repository.FirstOrDefaultAsync(e => e.Id == input.Id);
                    var region = await _RegionsRepository.FirstOrDefaultAsync(e => e.Id == input.RegionId);

                    if (name != null)
                    {
                        throw new UserFriendlyException("Hospital name already exists.");
                    }
                    if (hospitalId != null)
                    {
                        throw new UserFriendlyException("Hospital ID already exists.");
                    }

                    var hospital = new Hospital
                    {
                        Name = input.Name,
                        Id = input.Id,
                        TrustId = input.RegionId,
                        Postcode = input.Postcode,
                        ActiveDevMgt = input.ActiveDevMgt,
                        ShowButtonDevProc = input.ShowButtonDevProc
                    };

                    await Repository.InsertAsync(hospital);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    await SaveUserHospital(hospital, region);

                    await _dashboardAppService.UploadHospitalAsync(hospital);

                    if (!input.ActiveDevMgt)
                    {
                        await _specialtyInfoRepository.DeleteAsync(e => e.HospitalId == hospital.Id);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }                

                return input;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override async Task<HospitalDto> UpdateAsync(HospitalDto input)
        {
            try
            {
                var hospital = await Repository.FirstOrDefaultAsync(e => e.Id == input.Id);

                if (hospital == null)
                {
                    throw new UserFriendlyException("Hospital does not exists.");
                }

                var region = await _RegionsRepository.FirstOrDefaultAsync(e => e.Name == input.RegionName);

                if (input.TrustName != null)
                {
                    var trust = await _RegionsRepository.FirstOrDefaultAsync(e => e.Name == input.TrustName);
                    var id = Guid.NewGuid();
                    if (trust == null)
                    {
                        var newTrust = new Region
                        {
                            Id = id,
                            Name = input.TrustName,
                            Type = "Trust",
                            ParentId = region.Id,
                            IsEnabled = true
                        };

                        await _RegionsRepository.InsertAsync(newTrust);
                    }

                    hospital.TrustId = trust == null ? id : trust.Id;
                }

                hospital.Name = input.Name;
                hospital.Id = input.Id;
                hospital.Postcode = input.Postcode;
                hospital.ActiveDevMgt = input.ActiveDevMgt;
                hospital.ShowButtonDevProc = input.ShowButtonDevProc;
                await Repository.UpdateAsync(hospital);

                await SaveUserHospital(hospital, region);

                if (!input.ActiveDevMgt)
                {
                    await _specialtyInfoRepository.DeleteAsync(e => e.HospitalId == hospital.Id);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                return input;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RegionManagementNodeDto>> GetAllHospitalsMultiSelect(long? userId = null)
        {
            var hospitals = new List<HospitalDto>();
            var data = new List<RegionManagementNodeDto>();
            try
            {
                var userRealms = await _userRealmRepository.GetAll()
                .Where(e => e.UserId == userId)
                .Select(e => e.HospitalId)
                .ToListAsync();

                if (userId != null)
                {
                    hospitals = await Repository.GetAll()
                       .Where(e => userRealms.Contains(e.Id))
                       .Select(e => ObjectMapper.Map<HospitalDto>(e))
                       .ToListAsync();
                }
                else
                {
                    hospitals = await Repository.GetAll()
                   .Select(e => ObjectMapper.Map<HospitalDto>(e))
                   .ToListAsync();
                }

                data = hospitals.Select(r => new RegionManagementNodeDto
                {
                    Key = r.Id.ToString(),
                    Type = "person",
                    StyleClass = "p-person",
                    Label = r.Name,
                    Expanded = true,
                    Data = new RegionManagementDataDto
                    {
                        Name = r.Name,
                        DataType = "Hospital"
                    }
                }).ToList();

                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        private async Task SaveUserHospital(Hospital hospital, Region region)
        {
            var currentUser = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (currentUser != null)
            {
                var newUserHospital = new UserHospital();

                //Default activate to all superadmin
                var allSuperAdmins = await _userManager.GetUsersInRoleAsync(StaticRoleNames.Tenants.SuperAdmin);
                if (allSuperAdmins != null)
                {
                    if (allSuperAdmins.Count() > 0)
                    {
                        foreach (var user in allSuperAdmins)
                        {
                            newUserHospital = new UserHospital();
                            newUserHospital.HospitalId = hospital.Id;
                            newUserHospital.UserId = user.Id;

                            var getexistUH = await _userHospitalsRepository.GetAll()
                                .Where(_ => _.UserId == user.Id && _.HospitalId == hospital.Id).FirstOrDefaultAsync();
                            if (getexistUH == null)
                            {
                                await _userHospitalsRepository.InsertOrUpdateAsync(newUserHospital);
                            }

                            await SaveUserRealm(user.Id, region.Id, hospital.Id);
                        }

                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }

                //If admin, activate to other admin with samme region
                var roles = await _userManager.GetRolesAsync(currentUser);
                if (roles.Contains(StaticRoleNames.Tenants.Admin))
                {
                    //Current user admin
                    newUserHospital.HospitalId = hospital.Id;
                    newUserHospital.UserId = AbpSession.UserId.Value;

                    var getexistUH = await _userHospitalsRepository.GetAll()
                        .Where(_ => _.UserId == AbpSession.UserId.Value && _.HospitalId == hospital.Id).FirstOrDefaultAsync();

                    if (getexistUH == null)
                    {
                        await _userHospitalsRepository.InsertOrUpdateAsync(newUserHospital);
                    }

                    await SaveUserRealm(AbpSession.UserId.Value, region.Id, hospital.Id);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    //Get all users in region
                    var userRealm = await _userRealmRepository.GetAll()
                        .Include(_ => _.User)
                        .Include(_ => _.Region)
                        .Include(_ => _.Hospital)
                        .Where(_ => _.RegionId == region.Id).ToListAsync();

                    if (userRealm != null)
                    {
                        foreach (var userH in userRealm)
                        {
                            if (userH.User != null)
                            {
                                var thisRole = await _userManager.GetRolesAsync(userH.User);
                                if (thisRole.Contains(StaticRoleNames.Tenants.Admin))
                                {
                                    newUserHospital = new UserHospital();
                                    newUserHospital.HospitalId = hospital.Id;
                                    newUserHospital.UserId = userH.UserId ?? 0;

                                    var getexistUH1 = await _userHospitalsRepository.GetAll()
                                        .Where(_ => _.UserId == userH.UserId && _.HospitalId == hospital.Id).FirstOrDefaultAsync();
                                    if (getexistUH1 == null)
                                    {
                                        await _userHospitalsRepository.InsertOrUpdateAsync(newUserHospital);
                                    }

                                    await SaveUserRealm(userH.UserId ?? 0, region.Id, hospital.Id);
                                }
                            }                            
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }               
            }

        }

        private async Task SaveUserRealm(long userId, Guid regionId, string hospitalId)
        {
            bool saved = false;
            var data = new UserRealmMapping
            {
                UserId = userId,
                RegionId = regionId,
                HospitalId = null
            };

            var getExist1 = await _userRealmRepository.GetAll()
                .Where(_ => _.RegionId == regionId && _.HospitalId == null && _.UserId == userId).FirstOrDefaultAsync();

            if (getExist1 == null)
            {
                await _userRealmRepository.InsertOrUpdateAsync(data);
                saved = true;
            }

            data = new UserRealmMapping
            {
                UserId = userId,
                RegionId = null,
                HospitalId = hospitalId
            };

            var getExist2 = await _userRealmRepository.GetAll()
                .Where(_ => _.RegionId == null && _.HospitalId == hospitalId && _.UserId == userId).FirstOrDefaultAsync();

            if (getExist2 == null)
            {
                await _userRealmRepository.InsertOrUpdateAsync(data);
                saved = true;
            }

            if (saved)
            {
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

    }
}
