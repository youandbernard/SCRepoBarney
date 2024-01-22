using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using CaseMix.Aws.S3.Model;
using CaseMix.Aws.S3.Services;
using CaseMix.Core.Shared.Files;
using CaseMix.Core.Shared.Services;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.Manufactures.Dto;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SnomedApi;
using SnomedApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.Device
{
    public class DeviceAppService : AsyncCrudAppService<CaseMix.Entities.Device, DeviceDto, int, PagedDeviceResultRequestDto>, IDeviceAppService
    {
        private readonly AwsConfiguration _awsConfiguration;
        private readonly IRepository<Manufacture, Guid> _manufacturerRepository;
        private readonly IRepository<DocumentFile, int> _documentRepository;
        private readonly IRepository<GmdnAgencies, int> _gmdnRepository;
        private readonly IRepository<BodyStructure, int> _bodyStructuresRepository;
        private readonly IRepository<Hospital, string> _hospitalsRepository;
        private readonly IRepository<BodyStructureGroup, Guid> _bodyStructureGroupsRepository;
        private readonly IRepository<DeviceProcedure, int> _deviceProcedureRepository;
        private readonly IRepository<DeviceClass, Guid> _deviceClassRepository;
        private readonly IRepository<DeviceFamily, int> _deviceFamilyRepository;
        private readonly IRepository<DevicesHospital, int> _deviceHospitalRepository;
        private readonly IRepository<SpecialtyInfo, int> _specialtyInfoRepository;
        private readonly SnomedApiConfiguration _snomedApiConfiguration;

        private readonly UserManager _userManager;
        private readonly IS3Service _s3Service;
        private readonly IEmailService _emailService;

        public DeviceAppService(
            IOptions<SnomedApiConfiguration> snomedApiConfiguration,
            IS3Service s3Service,
            IEmailService emailService,
            UserManager userManager,
            IOptions<AwsConfiguration> awsConfiguration,
            IRepository<Manufacture, Guid> manufacturerRepository,
            IRepository<DocumentFile, int> documentRepository,
            IRepository<CaseMix.Entities.Device, int> deviceRepository,
            IRepository<GmdnAgencies, int> gmdnRepository,
            IRepository<BodyStructure, int> bodyStructuresRepository,
            IRepository<Hospital, string> hospitalsRepository,
            IRepository<BodyStructureGroup, Guid> bodyStructureGroupsRepository,
            IRepository<DeviceProcedure, int> deviceProcedureRepository,
            IRepository<DeviceClass, Guid> deviceClassRepository,
            IRepository<DeviceFamily, int> deviceFamilyRepository,
            IRepository<DevicesHospital, int> deviceHospitalRepository,
            IRepository<SpecialtyInfo, int> specialtyInfoRepository
            ) : base(deviceRepository)
        {
            _s3Service = s3Service;
            _emailService = emailService;
            _userManager = userManager;
            _manufacturerRepository = manufacturerRepository;
            _documentRepository = documentRepository;
            _gmdnRepository = gmdnRepository;
            _bodyStructuresRepository = bodyStructuresRepository;
            _hospitalsRepository = hospitalsRepository;
            _bodyStructureGroupsRepository = bodyStructureGroupsRepository;
            _deviceProcedureRepository = deviceProcedureRepository;
            _deviceClassRepository = deviceClassRepository;
            _deviceFamilyRepository = deviceFamilyRepository;
            _deviceHospitalRepository = deviceHospitalRepository;
            _specialtyInfoRepository = specialtyInfoRepository;
            _snomedApiConfiguration = snomedApiConfiguration.Value;

            _awsConfiguration = awsConfiguration.Value;
        }

        protected override IQueryable<CaseMix.Entities.Device> CreateFilteredQuery(PagedDeviceResultRequestDto input)
        {
            try
            {
                var currentUser = _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
                var roles = _userManager.GetRolesAsync(currentUser.Result);

                if (roles.Result.Contains(StaticRoleNames.Tenants.Manufacturer))
                {
                    var result = base.CreateFilteredQuery(input)
                        .Include(_ => _.Manufacturer)
                        .Include(_ => _.BodyStructureGroup)
                        .Include(_ => _.DeviceClass)
                        .Where(_ => _.ManufacturerId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        .Where(_ => _.ManufacturerId == currentUser.Result.ManufactureId)
                        .WhereIf(input.DisabledOnly == true, e => e.Status == 0)
                        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                        e => e.DeviceName.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Id.ToString().Contains(input.Keyword.ToLower()) ||
                            e.GMDNTermCode.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Model.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.BrandName.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.GS1Code.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.CreatedDate.ToString().ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Manufacturer.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.BodyStructureGroup.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.DeviceClass.Class.ToLower().Contains(input.Keyword.ToLower())
                        );

                    return result;
                }
                else if ((roles.Result.Contains(StaticRoleNames.Tenants.Surgeon) || roles.Result.Contains(StaticRoleNames.Tenants.Admin))
                        && !roles.Result.Contains(StaticRoleNames.Tenants.SuperAdmin))
                {
                    var result = base.CreateFilteredQuery(input)
                        .Include(_ => _.Manufacturer)
                        .Include(_ => _.BodyStructureGroup)
                        .Include(_ => _.DeviceClass)
                        .Where(_ => _.Status == (int)DeviceStatus.Active && _.ManufacturerId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        .Where(_ =>
                            _specialtyInfoRepository.GetAll().Any(e => e.HospitalId == input.hospitalId && e.SpecialtyId == _.BodyStructureGroupId)
                        )
                        .WhereIf(input.BodyStructureGroupId.HasValue, e => e.BodyStructureGroup.Id == input.BodyStructureGroupId)
                        .WhereIf(input.ManufacturerId.HasValue, e => e.Manufacturer.Id == input.ManufacturerId)
                        .WhereIf(input.DeviceClassId.HasValue, e => e.DeviceClass.Id == input.DeviceClassId)
                        .WhereIf(input.UnAvailableOnly == true, e =>
                            !_deviceHospitalRepository.GetAll().Any(d => d.DeviceId == e.Id && d.HospitalId == input.hospitalId)
                        )
                        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                        e => e.DeviceName.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Id.ToString().Contains(input.Keyword.ToLower()) ||
                            e.GMDNTermCode.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Model.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.BrandName.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.GS1Code.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.CreatedDate.ToString().ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Manufacturer.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.BodyStructureGroup.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.DeviceClass.Class.ToLower().Contains(input.Keyword.ToLower())
                        );

                    if (!string.IsNullOrWhiteSpace(input.hospitalId))
                    {
                        if (!input.UnAvailableOnly)
                        {
                            result.ToList().ForEach(_ =>
                                _.IsAvailable = _deviceHospitalRepository.GetAll().Any(d => d.DeviceId == _.Id && d.HospitalId == input.hospitalId)
                            );
                        }
                        else
                        {
                            result.ToList().ForEach(_ =>
                                _.IsAvailable = false
                            );
                        }

                    }

                    return result;
                }


                //Super Admin - Show all devices
                var ret = base.CreateFilteredQuery(input)
                    .Include(_ => _.Manufacturer)
                    .Include(_ => _.BodyStructureGroup)
                    .Include(_ => _.DeviceClass)
                    .Where(_ => _.ManufacturerId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    .WhereIf(input.BodyStructureGroupId.HasValue, e => e.BodyStructureGroup.Id == input.BodyStructureGroupId)
                    .WhereIf(input.ManufacturerId.HasValue, e => e.Manufacturer.Id == input.ManufacturerId)
                    .WhereIf(input.DeviceClassId.HasValue, e => e.DeviceClass.Id == input.DeviceClassId)
                    .WhereIf(input.DisabledOnly == true, e => e.Status == 0)
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                        e => e.DeviceName.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Id.ToString().Contains(input.Keyword.ToLower()) ||
                            e.GMDNTermCode.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Model.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.BrandName.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.GS1Code.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.CreatedDate.ToString().ToLower().Contains(input.Keyword.ToLower()) ||
                            e.Manufacturer.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.BodyStructureGroup.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                            e.DeviceClass.Class.ToLower().Contains(input.Keyword.ToLower())
                        );

                ret.ToList().ForEach(_ => _.SuperAdminUser = true);

                if (!string.IsNullOrWhiteSpace(input.hospitalId))
                {
                    ret.ToList().ForEach(_ =>
                        _.IsAvailable = _deviceHospitalRepository.GetAll().Any(d => d.DeviceId == _.Id && d.HospitalId == input.hospitalId)
                    );
                }

                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> EnableDisableDevice(int deviceId, int stat)
        {
            bool ret = false;

            var device = await Repository.GetAll()
               .Where(_ => _.Id == deviceId).FirstOrDefaultAsync();

            try
            {
                if (device != null)
                {
                    device.Status = stat == 0 ? 1 : 0;
                    device.ModifiedDate = DateTime.UtcNow;
                    device.ModifiedBy = AbpSession.UserId.Value;

                    await Repository.UpdateAsync(device);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    ret = true;
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <param name="stat"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<bool> DevicesAvailability(List<int> deviceIds, int stat, string hospitalId)
        {
            try
            {
                if (deviceIds.Count > 0)
                {
                    var notExists = deviceIds.Where(c => !_deviceHospitalRepository.GetAll()
                             .Any(d => d.DeviceId == c && d.HospitalId == hospitalId)).ToList();

                    var exists = deviceIds.Where(c => _deviceHospitalRepository.GetAll()
                           .Any(d => d.DeviceId == c && d.HospitalId == hospitalId)).ToList();

                    if (stat == 1)
                    {
                        if (notExists.Count > 0)
                        {
                            foreach (var e in notExists)
                            {
                                var deviceHospital = new DevicesHospital
                                {
                                    DeviceId = e,
                                    HospitalId = hospitalId,
                                    UserId = AbpSession.UserId.Value,
                                    CreatedDate = DateTime.UtcNow
                                };

                                await _deviceHospitalRepository.InsertAsync(deviceHospital);
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        if (exists.Count > 0)
                        {
                            foreach (var e in exists)
                            {
                                var deviceHospital = await _deviceHospitalRepository.GetAll()
                                    .Where(_ => _.DeviceId == e && _.HospitalId == hospitalId).FirstOrDefaultAsync();

                                if (deviceHospital != null)
                                    await _deviceHospitalRepository.DeleteAsync(deviceHospital);
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        public async Task<bool> EnableDisableSelected(List<int> deviceIds, int stat)
        {
            bool ret = false;

            try
            {
                if (deviceIds != null)
                {
                    if (deviceIds.Count > 0)
                    {
                        foreach (int deviceId in deviceIds)
                        {
                            var device = await Repository.GetAll().Where(_ => _.Id == deviceId).FirstOrDefaultAsync();
                            if (device != null)
                            {
                                device.Status = stat;
                                device.ModifiedDate = DateTime.UtcNow;
                                device.ModifiedBy = AbpSession.UserId.Value;

                                await Repository.UpdateAsync(device);
                                await CurrentUnitOfWork.SaveChangesAsync();

                                ret = true;
                            }
                        }

                        //remove as not available to any hospitals if disabled
                        if (stat == 0)
                        {
                            var exists = deviceIds.Where(c => _deviceHospitalRepository.GetAll().Any(d => d.DeviceId == c)).ToList();
                            if (exists.Count > 0)
                            {
                                foreach (var e in exists)
                                {
                                    var deviceHospitals = await _deviceHospitalRepository.GetAll().Where(_ => _.DeviceId == e).ToListAsync();
                                    if (deviceHospitals != null)
                                    {
                                        if (deviceHospitals.Count > 0)
                                        {
                                            foreach(var devicehospital in deviceHospitals)
                                            {
                                                await _deviceHospitalRepository.DeleteAsync(devicehospital);
                                                await CurrentUnitOfWork.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        public async Task<IEnumerable<DeviceClassDto>> GetAllDeviceClass()
        {
            var deviceClass = await _deviceClassRepository.GetAll()
                .OrderBy(e => e.Class)
                .Select(e => ObjectMapper.Map<DeviceClassDto>(e))
                .ToListAsync();

            return deviceClass;
        }

        public async Task<DeviceDto> GetByDeviceId(int id)
        {
            try
            {
                var device = await Repository.GetAll()
                    .Where(_ => _.Id == id)
                    .Select(e => ObjectMapper.Map<DeviceDto>(e))
                    .FirstOrDefaultAsync();

                return device;
            }
            catch (Exception Ex)
            {
                return new DeviceDto();
            }
        }

        public async Task<IEnumerable<DevicesTermCodeDto>> GetByDeviceGMDNCode(string gmdntermCode)
        {
            try
            {
                var device = await Repository.GetAll()
                    .Where(_ => _.GMDNTermCode == gmdntermCode)
                    .Select(e => ObjectMapper.Map<DeviceDto>(e)).FirstOrDefaultAsync();

                var results = new List<DevicesTermCodeDto>();

                if (device != null)
                {
                    var dtc = new DevicesTermCodeDto();

                    dtc.DeviceName = device.DeviceName;
                    dtc.BrandName = device.BrandName;
                    dtc.DeviceDescription = device.DeviceDescription;
                    dtc.GMDNTermCode = device.GMDNTermCode;
                    dtc.ManufacturerId = device.ManufacturerId;
                    dtc.ManufacturerName =
                        _manufacturerRepository.GetAll()
                            .Where(m => m.Id == device.ManufacturerId)
                            .Select(s => s.Name).FirstOrDefault();

                    results.Add(dtc);
                }

                if (results.Count > 0)
                {
                    List<string> groupedCode = results.GroupBy(g => g.GMDNTermCode).Select(_ => _.Key).ToList();
                    if (groupedCode.Count > 0)
                    {
                        foreach (var r in groupedCode)
                        {
                            var gmdn = await _gmdnRepository.GetAll()
                                .Where(_ => _.Code == r).FirstOrDefaultAsync();

                            var fromResults = results.Where(_ => _.GMDNTermCode == r).ToList();

                            if (gmdn != null)
                            {
                                foreach (var g in fromResults)
                                {
                                    g.GMDNStatus = gmdn.Status;
                                    g.GMDNDefinition = string.IsNullOrWhiteSpace(gmdn.Definition) ? g.DeviceDescription : gmdn.Definition;
                                    g.GMDNTermIsIVD = gmdn.TermIsIVD;
                                    g.GMDNCreatedDate = gmdn.CreatedDate;
                                    g.GMDNObsoletedDate = gmdn.ObsoletedDate;
                                }
                            }
                            else
                            {
                                foreach (var g in fromResults)
                                {
                                    g.GMDNDefinition = g.DeviceDescription;
                                }
                            }
                        }
                    }
                }

                return results.AsEnumerable();
            }
            catch (Exception Ex)
            {
                return new List<DevicesTermCodeDto>();
            }
        }

        public async Task<IEnumerable<DevicesTermCodeDto>> GetByDeviceCode(string gmdntermCode)
        {
            try
            {
                var devices = await Repository.GetAll()
                    .Where(_ => _.GMDNTermCode == gmdntermCode)
                    .Select(e => ObjectMapper.Map<DeviceDto>(e)).ToListAsync();

                var results = new List<DevicesTermCodeDto>();

                if (devices != null)
                {
                    devices.ForEach(_ => results.Add(new DevicesTermCodeDto
                    {
                        DeviceName = _.DeviceName,
                        BrandName = _.BrandName,
                        DeviceDescription = _.DeviceDescription,
                        GMDNTermCode = _.GMDNTermCode,
                        ManufacturerId = _.ManufacturerId,
                        ManufacturerName =
                            _manufacturerRepository.GetAll()
                                .Where(m => m.Id == _.ManufacturerId)
                                .Select(s => s.Name).FirstOrDefault()

                    }));
                }

                if (results.Count > 0)
                {
                    List<string> groupedCode = results.GroupBy(g => g.GMDNTermCode).Select(_ => _.Key).ToList();
                    if (groupedCode.Count > 0)
                    {
                        foreach (var r in groupedCode)
                        {
                            var gmdn = await _gmdnRepository.GetAll()
                                .Where(_ => _.Code == r).FirstOrDefaultAsync();

                            var fromResults = results.Where(_ => _.GMDNTermCode == r).ToList();

                            if (gmdn != null)
                            {
                                foreach (var g in fromResults)
                                {
                                    g.GMDNStatus = gmdn.Status;
                                    g.GMDNDefinition = string.IsNullOrWhiteSpace(gmdn.Definition) ? g.DeviceDescription : gmdn.Definition;
                                    g.GMDNTermIsIVD = gmdn.TermIsIVD;
                                    g.GMDNCreatedDate = gmdn.CreatedDate;
                                    g.GMDNObsoletedDate = gmdn.ObsoletedDate;
                                }
                            }
                            else
                            {
                                foreach (var g in fromResults)
                                {
                                    g.GMDNDefinition = g.DeviceDescription;
                                }
                            }
                        }
                    }
                }

                return results.AsEnumerable();
            }
            catch (Exception Ex)
            {
                return new List<DevicesTermCodeDto>();
            }
        }

        public async Task UploadFile([FromForm] FileDto fileInput)
        {
            var file = fileInput.File;
            var fileName = file.FileName;

            var dt = FileHelper.CSVtoDataTableParser(file);

            #region Upload to S3 and log files

            int docFileId = 0;

            var currentUser = _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            string folder = "manufacturers";

            await using (var stream = file.OpenReadStream())
            {
                var fileBytes = stream.GetAllBytes();
                try
                {
                    Logger.Debug($"Uploading filename: [{file.FileName}] with [{file.Length / 1024f}] MB of size.");

                    await _s3Service.UploadAsync(fileName, fileBytes, folder, AbpSession.UserId.Value, true);

                    Logger.Debug($"Uploaded filename: [{file.FileName}] with [{file.Length / 1024f}] MB of size.");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error Uploading filename: [{file.FileName}] with [{file.Length / 1024f}] MB of size.", ex);
                    throw new UserFriendlyException(ex.Message);
                }

                fileName = $"{folder}/{AbpSession.UserId.Value}/files/{fileName}";

                var document = new DocumentFile
                {
                    Filename = fileName,
                    Filepath = _awsConfiguration.S3Bucket, //bucket name
                    Filelength = (long)file.Length,
                    Filetype = file.ContentType,
                    DocumentType = (int)DocumentType.Device,
                    Enable = true,
                    Active = true,
                    UserId = AbpSession.UserId.Value,
                    DateUploaded = DateTime.UtcNow,
                    ManufacturerId = currentUser.Result.ManufactureId
                };

                await _documentRepository.InsertAsync(document);
                await CurrentUnitOfWork.SaveChangesAsync();

                docFileId = document.Id;
            }

            #endregion

            #region Save file data to DB          

            var devicesDto = FileHelper.DataTableToModel<DeviceDto>(dt);
            List<string> uniqueUDI = null;
            uniqueUDI = devicesDto.Select(u => u.UID).Distinct().ToList();

            List<DeviceDto> deviceDtos = new List<DeviceDto>();
            if (uniqueUDI != null)
            {
                if (uniqueUDI.Count > 0)
                {
                    foreach (var d in devicesDto.Where(p => !deviceDtos.Any(p2 => p2.UID == p.UID)))
                    {
                        deviceDtos.Add(d);
                    }
                }
            }

            if (deviceDtos != null && deviceDtos.Count > 0)
            {
                foreach (var deviceDto in deviceDtos)
                {
                    int deviceId = 0;

                    var device = new CaseMix.Entities.Device
                    {
                        UID = deviceDto.UID,
                        GMDNTermCode = deviceDto.GMDNTermCode,
                        DeviceName = deviceDto.DeviceName,
                        DeviceDescription = deviceDto.DeviceDescription,
                        BrandName = deviceDto.BrandName,
                        Model = deviceDto.Model,
                        ManufacturerId = currentUser.Result.ManufactureId ?? new Guid(),
                        DocFileId = docFileId,
                        Status = (int)DeviceStatus.Disabled,
                        UserId = AbpSession.UserId.Value,
                        CreatedDate = DateTime.UtcNow,
                        BodyStructureGroupId = deviceDto.BodyStructureGroupId,
                        DeviceClassId = deviceDto.DeviceClassId,
                        DeviceFamilyId = deviceDto.DeviceFamilyId,
                        GTINCode = deviceDto.GTINCode
                    };

                    var existDevice = await Repository.GetAll()
                        .Where(_ => _.UID.Trim().ToLower() == device.UID.Trim().ToLower()).FirstOrDefaultAsync();
                    if (existDevice != null)
                    {
                        existDevice.UID = device.UID;
                        existDevice.GMDNTermCode = device.GMDNTermCode;
                        existDevice.DeviceName = device.DeviceName;
                        existDevice.DeviceDescription = device.DeviceDescription;
                        existDevice.BrandName = device.BrandName;
                        existDevice.Model = device.Model;
                        existDevice.ManufacturerId = device.ManufacturerId;
                        existDevice.DocFileId = device.DocFileId;
                        existDevice.Status = device.Status;
                        existDevice.UserId = device.UserId;
                        existDevice.CreatedDate = device.CreatedDate;
                        existDevice.BodyStructureGroupId = device.BodyStructureGroupId;
                        existDevice.DeviceClassId = device.DeviceClassId;
                        existDevice.DeviceFamilyId = device.DeviceFamilyId;
                        existDevice.GTINCode = device.GTINCode;
                        existDevice.ModifiedBy = AbpSession.UserId.Value;
                        existDevice.ModifiedDate = DateTime.UtcNow;

                        existDevice = await Repository.UpdateAsync(existDevice);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        deviceId = existDevice.Id;
                    }
                    else
                    {
                        device = await Repository.InsertAsync(device);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        deviceId = device.Id;
                    }

                    //Remove DeviceProcedures of deviceid if exist
                    var getDeviceProcByDeviceId = await _deviceProcedureRepository.GetAll().Where(_ => _.DeviceId == deviceId).ToListAsync();
                    if (getDeviceProcByDeviceId != null)
                    {
                        if (getDeviceProcByDeviceId.Count > 0)
                        {
                            foreach (var proc in getDeviceProcByDeviceId)
                            {
                                await _deviceProcedureRepository.DeleteAsync(proc);
                            }
                        }
                    }
                    await CurrentUnitOfWork.SaveChangesAsync();


                    #region Save DeviceProcedures

                    //Default hospital
                    //var szegedHospital = await _hospitalsRepository.GetAll().Where(_ => _.Name.ToLower().Contains("szeged")).FirstOrDefaultAsync();

                    var deviceProcedures = new List<DeviceProcedure>();
                    var deviceProcedure = new DeviceProcedure();

                    deviceProcedure.DeviceId = deviceId;
                    deviceProcedure.BodyStructureGroupId = deviceDto.BodyStructureGroupId.HasValue ? deviceDto.BodyStructureGroupId.Value : new Guid("00000000-0000-0000-000000000000");
                    deviceProcedure.BodyStructureId = null;
                    deviceProcedure.BodyStructureProcId = null;
                    deviceProcedure.UserId = AbpSession.UserId.Value;
                    deviceProcedure.CreatedDate = DateTime.UtcNow;
                    deviceProcedure.SnomedID = null;
                    //if (szegedHospital != null)
                    //{
                    //    deviceProcedure.HospitalId = szegedHospital.Id;
                    //}

                    //deviceProcedures.Add(deviceProcedure);
                    await _deviceProcedureRepository.InsertAsync(deviceProcedure);

                    var bodyStructureGroup = await _bodyStructureGroupsRepository.GetAll()
                                    .Include(_ => _.BodyStructures)
                                        .ThenInclude(_ => _.BodyStructureSubProcedures)
                                    .Where(_ => _.Id == deviceDto.BodyStructureGroupId).FirstOrDefaultAsync();

                    List<BodyStructure> bodyStructures = new List<BodyStructure>();
                    if (bodyStructureGroup != null)
                    {
                        bodyStructures = bodyStructureGroup.BodyStructures.ToList();
                    }

                    if (bodyStructures != null)
                    {
                        if (bodyStructures.Count > 0)
                        {
                            foreach (var bs in bodyStructures)
                            {
                                deviceProcedure = new DeviceProcedure();
                                deviceProcedure.DeviceId = deviceId;
                                deviceProcedure.BodyStructureGroupId = deviceDto.BodyStructureGroupId.HasValue ? deviceDto.BodyStructureGroupId.Value : new Guid("00000000-0000-0000-000000000000");
                                deviceProcedure.BodyStructureId = bs.Id;
                                deviceProcedure.BodyStructureProcId = null;
                                deviceProcedure.UserId = AbpSession.UserId.Value;
                                deviceProcedure.CreatedDate = DateTime.UtcNow;
                                deviceProcedure.SnomedID = null;
                                //if (szegedHospital != null)
                                //{
                                //    deviceProcedure.HospitalId = szegedHospital.Id;
                                //}

                                //deviceProcedures.Add(deviceProcedure);
                                await _deviceProcedureRepository.InsertAsync(deviceProcedure);

                                var bodyStructureProc = bs.BodyStructureSubProcedures.ToList();

                                if (_snomedApiConfiguration.IsEnable == "1")
                                {
                                    try
                                    {
                                        //Get Snomed  list of BodyStructureId
                                        using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
                                        {
                                            var conceptResult = await snomedApi.FindConceptsUsingGETMethodAsync(_snomedApiConfiguration.Branch, ecl: await GetSnomedQuery(bs.Id), limit: 1000);
                                            if (conceptResult != null && conceptResult.Items != null)
                                            {
                                                var conceptResultItems = (IList)conceptResult.Items;
                                                var exempted = string.Join(",", Enum.GetValues(typeof(ExemptedBodyStructures)).Cast<int>()).Split(",");
                                                var isExempted = exempted.Any(e => e == bs.Id.ToString());

                                                if (!isExempted)
                                                {
                                                    if (conceptResultItems != null)
                                                    {
                                                        if (conceptResultItems.Count > 0)
                                                        {
                                                            foreach (var conceptResultItem in conceptResultItems)
                                                            {
                                                                var jConceptResultItemObject = conceptResultItem as JObject;
                                                                var concept = jConceptResultItemObject.ToObject<Concept>();

                                                                if (!bodyStructureProc.Where(_ => _.SnomedId == concept.ConceptId).Any())
                                                                {
                                                                    BodyStructureSubProcedure _ = new BodyStructureSubProcedure();
                                                                    _.BodyStructure = bs;
                                                                    _.BodyStructureId = bs.Id;
                                                                    _.SnomedId = concept.ConceptId;
                                                                    _.Description = concept.Fsn.Term;

                                                                    bodyStructureProc.Add(_);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex.Message);
                                    }
                                }

                                if (bodyStructureProc != null)
                                {
                                    if (bodyStructureProc.Count > 0)
                                    {
                                        foreach (var bsp in bodyStructureProc)
                                        {
                                            deviceProcedure = new DeviceProcedure();
                                            deviceProcedure.DeviceId = deviceId;
                                            deviceProcedure.BodyStructureGroupId = deviceDto.BodyStructureGroupId.HasValue ? deviceDto.BodyStructureGroupId.Value : new Guid("00000000-0000-0000-000000000000");
                                            deviceProcedure.BodyStructureId = bs.Id;

                                            if (bsp.Id > 0)
                                                deviceProcedure.BodyStructureProcId = bsp.Id;
                                            else
                                                deviceProcedure.BodyStructureProcId = null;

                                            deviceProcedure.UserId = AbpSession.UserId.Value;
                                            deviceProcedure.CreatedDate = DateTime.UtcNow;
                                            deviceProcedure.SnomedID = bsp.SnomedId;
                                            //if (szegedHospital != null)
                                            //{
                                            //    deviceProcedure.HospitalId = szegedHospital.Id;
                                            //}

                                            //deviceProcedures.Add(deviceProcedure);
                                            await _deviceProcedureRepository.InsertAsync(deviceProcedure);
                                        }
                                    }
                                }
                            }

                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }

                    #endregion                    
                }
            }

            #endregion
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetBodyStructureGroups()
        {
            var bodyStructureGroups = await Repository.GetAll()
            .Where(e => e.BodyStructureGroupId != null)
            .Select(e => ObjectMapper.Map<BodyStructureGroupDto>(e.BodyStructureGroup))
            .Distinct()
            .ToListAsync();

            return bodyStructureGroups;
        }

        public async Task<IEnumerable<ManufactureDto>> GetManufacturers()
        {
            var manufacturers = await Repository.GetAll()
            .Where(e => e.ManufacturerId != null)
            .Select(e => ObjectMapper.Map<ManufactureDto>(e.Manufacturer))
            .Distinct()
            .ToListAsync();

            return manufacturers;
        }

        public async Task<IEnumerable<DeviceClassDto>> GetDeviceClass()
        {
            var deviceClass = await Repository.GetAll()
            .Where(e => e.DeviceClassId != null)
            .Select(e => ObjectMapper.Map<DeviceClassDto>(e.DeviceClass))
            .Distinct()
            .ToListAsync();

            return deviceClass;
        }

        public async Task<IEnumerable<DeviceFamilyDto>> GetAllDeviceFamily(Guid bodyStructureGroupId)
        {
            List<DeviceFamilyDto> deviceFamily = null;

            deviceFamily = await _deviceFamilyRepository.GetAll()
                .Where(_ => _.BodyStructureGroupId == null)
                .OrderBy(e => e.Name)
                .Select(e => ObjectMapper.Map<DeviceFamilyDto>(e))
                .ToListAsync();

            if (bodyStructureGroupId != null)
            {
                deviceFamily = await _deviceFamilyRepository.GetAll()
                    .Where(_ => _.BodyStructureGroupId == bodyStructureGroupId)
                    .OrderBy(e => e.Name)
                    .Select(e => ObjectMapper.Map<DeviceFamilyDto>(e))
                    .ToListAsync();
            }

            return deviceFamily;
        }

        public byte[] GenerateDeviceCSVTemplate(Guid bodyStructureGroupId, int deviceFamilyId, Guid deviceClassId)
        {
            var sampleRecord = new DeviceTemplateDto()
            {
                UDI = "250817",
                GMDNTermCode = "10014",
                DeviceName = "Road Runner 1000",
                DeviceDescription = "Cayote proof Road Runner Trap",
                BrandName = "Acme",
                Model = "RR1",
                Specialty = bodyStructureGroupId,
                DeviceFamily = deviceFamilyId,
                Class = deviceClassId,
                GTINCode = 5012345670003
            };

            MemoryStream GetCsvMemStream(DeviceTemplateDto rec)
            {
                var uTF8Encoding = new System.Text.UTF8Encoding(true);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = uTF8Encoding,
                    ShouldQuote = _ => false,
                    TrimOptions = TrimOptions.Trim
                };

                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms, uTF8Encoding))
                using (var cw = new CsvHelper.CsvWriter(sw, config))
                {
                    cw.WriteHeader<DeviceTemplateDto>();
                    cw.NextRecord();
                    cw.WriteRecord(rec);
                    return ms;
                }
            }

            return GetCsvMemStream(sampleRecord).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bodyStructureId"></param>
        /// <returns></returns>
        private async Task<string> GetSnomedQuery(int bodyStructureId)
        {
            var bodyStructure = await _bodyStructuresRepository.GetAll()
                .Where(e => e.Id == bodyStructureId)
                .Include(e => e.BodyStructureQueries)
                .FirstOrDefaultAsync();

            var queries = new List<string>();
            foreach (var query in bodyStructure.BodyStructureQueries.OrderBy(e => e.QueryOrder))
            {
                if (!query.QuerySimplified.IsNullOrWhiteSpace())
                {
                    queries.Add($"({query.QuerySimplified})");
                }
            }

            string sQuery = string.Join(" OR ", queries);
            return sQuery;
        }
    }
}
