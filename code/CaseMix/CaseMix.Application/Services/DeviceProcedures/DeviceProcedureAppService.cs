using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using CaseMix.Core.Shared.Models;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Services.BodyStructureGroups.Dto;
using CaseMix.Services.BodyStructureProcedures.Dto;
using CaseMix.Services.BodyStructures.Dto;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.DeviceProcedures.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SnomedApi;
using SnomedApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.DeviceProcedures
{
    public class DeviceProcedureAppService : CaseMixAppServiceBase, IDeviceProcedureAppService
    {
        private readonly SnomedApiConfiguration _snomedApiConfiguration;
        private readonly IRepository<BodyStructureGroup, Guid> _bodyStructureGroupsRepository;
        private readonly IRepository<BodyStructure> _bodyStructuresRepository;
        private readonly IRepository<DeviceProcedure, int> _deviceProcedureRepository;
        private readonly IRepository<BodyStructureSubProcedure, int> _bodyStructureSubProcedureRepository;
        private readonly IRepository<SpecialtyInfo, int> _specialtyInfoRepository;
        private readonly IRepository<DevicesHospital, int> _deviceHospitalRepository;

        public DeviceProcedureAppService(
            IOptions<SnomedApiConfiguration> snomedApiConfiguration,
            IRepository<BodyStructureSubProcedure, int> bodyStructureSubProcedureRepository,
            IRepository<DeviceProcedure, int> deviceProcedureRepository,
            IRepository<BodyStructureGroup, Guid> bodyStructureGroupsRepository,
            IRepository<BodyStructure> bodyStructuresRepository,
            IRepository<SpecialtyInfo, int> specialtyInfoRepository,
            IRepository<DevicesHospital, int> deviceHospitalRepository)
        {
            _snomedApiConfiguration = snomedApiConfiguration.Value;
            _bodyStructureSubProcedureRepository = bodyStructureSubProcedureRepository;
            _deviceProcedureRepository = deviceProcedureRepository;
            _bodyStructureGroupsRepository = bodyStructureGroupsRepository;
            _bodyStructuresRepository = bodyStructuresRepository;
            _specialtyInfoRepository = specialtyInfoRepository;
            _deviceHospitalRepository = deviceHospitalRepository;
        }

        public async Task<IEnumerable<BodyStructureGroupDto>> GetByDeviceId(int deviceId, string hospitalId)
        {
            var deviceProcedures = await _deviceProcedureRepository.GetAll()
                .Where(_ => _.DeviceId == deviceId && _.HospitalId == hospitalId)
                .Include(_ => _.User)
                .Include(_ => _.BodyStructureGroup)
                .Include(_ => _.BodyStructure)
                    .ThenInclude(_ => _.BodyStructureGroup)
                .Include(_ => _.BodyStructureSubProcedure)
                .Select(_ => ObjectMapper.Map<DeviceProcedureDto>(_))
                .ToListAsync();

            List<BodyStructureGroupDto> bodyStructureGroupDtos = new List<BodyStructureGroupDto>();
            List<BodyStructureDto> bodyStructureDtos = new List<BodyStructureDto>();
            List<BodyStructureSubProcedureDto> bodyStructureSubProcedureDtos = new List<BodyStructureSubProcedureDto>();

            List<Guid> bodyStructureGroups = new List<Guid>();

            bodyStructureGroups = deviceProcedures.GroupBy(_ => _.BodyStructureGroupId)
                .Select(_ => _.Key).ToList();

            foreach (var grp in bodyStructureGroups)
            {
                bodyStructureDtos = new List<BodyStructureDto>();

                var deviceProc = deviceProcedures.Where(_ => _.BodyStructureGroupId == grp);
                bodyStructureGroupDtos.Add(deviceProc.Select(_ => _.BodyStructureGroup).FirstOrDefault());

                var bodyStructures = deviceProc.GroupBy(_ => _.BodyStructureId)
                    .Select(_ => _.Select(s => s.BodyStructure).FirstOrDefault());

                foreach (var bs in bodyStructures)
                {
                    bodyStructureSubProcedureDtos = new List<BodyStructureSubProcedureDto>();

                    if (bs != null)
                    {
                        bodyStructureDtos.Add(bs);

                        var bodyStructureSubProc = deviceProc.Where(_ => _.BodyStructureId == bs.Id);

                        List<BodyStructureSubProcedureDto> snomedSubProcedures = new List<BodyStructureSubProcedureDto>();
                        List<BodyStructureSubProcedureDto> fromDBSubProcedures = new List<BodyStructureSubProcedureDto>();

                        var subProcedures = await _bodyStructureSubProcedureRepository.GetAll()
                            .Where(_ => _.BodyStructureId == bs.Id).ToListAsync();

                        if (subProcedures != null)
                        {
                            foreach (var sp in subProcedures)
                            {
                                BodyStructureSubProcedureDto _ = new BodyStructureSubProcedureDto();
                                _.BodyStructure = bs;
                                _.BodyStructureId = bs.Id;
                                _.SnomedId = sp.SnomedId;
                                _.Description = sp.Description;

                                fromDBSubProcedures.Add(_);
                            }
                        }

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

                                                    BodyStructureSubProcedureDto _ = new BodyStructureSubProcedureDto();
                                                    _.BodyStructure = bs;
                                                    _.BodyStructureId = bs.Id;
                                                    _.SnomedId = concept.ConceptId;
                                                    _.Description = concept.Fsn.Term;

                                                    snomedSubProcedures.Add(_);
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

                        foreach (var sProc in bodyStructureSubProc)
                        {
                            if (sProc.BodyStructureSubProcedure == null)
                            {
                                var getSubProcedureFromSnomed = snomedSubProcedures
                                    .Where(_ => _.BodyStructureId == sProc.BodyStructureId &&
                                    _.SnomedId == sProc.SnomedID).FirstOrDefault();

                                if (getSubProcedureFromSnomed != null)
                                {
                                    bodyStructureSubProcedureDtos.Add(getSubProcedureFromSnomed);
                                }
                                else
                                {
                                    var getSubProcedureFromDB = fromDBSubProcedures
                                        .Where(_ => _.BodyStructureId == sProc.BodyStructureId &&
                                        _.SnomedId == sProc.SnomedID).FirstOrDefault();

                                    bodyStructureSubProcedureDtos.Add(getSubProcedureFromDB);
                                }
                            }
                            else
                            {
                                bodyStructureSubProcedureDtos.Add(sProc.BodyStructureSubProcedure);
                            }
                        }

                        foreach (var bsDto in bodyStructureDtos)
                        {
                            if (bsDto.Id == bs.Id)
                            {
                                bsDto.BodyStructureSubProcedures = bodyStructureSubProcedureDtos;
                            }
                        }
                    }
                }

                foreach (var bsg in bodyStructureGroupDtos)
                {
                    if (bsg.Id == grp)
                    {
                        bsg.BodyStructures = bodyStructureDtos;
                    }
                }
            }

            return bodyStructureGroupDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeNodes"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task SaveSelectedDeviceProcedures(int deviceId, IEnumerable<TreeNodeInput> treeNodes)
        {
            List<TreeNodeInput> DoneNodes = new List<TreeNodeInput>();

            List<DeviceProcedure> addedDeviceProcedures = new List<DeviceProcedure>();
            List<DeviceProcedure> existingDeviceProcedures = new List<DeviceProcedure>();
            existingDeviceProcedures = await _deviceProcedureRepository.GetAll()
                .Where(_ => _.DeviceId == deviceId).ToListAsync();

            if (treeNodes != null && treeNodes.Count() > 0)
            {
                DeviceProcedure deviceProcedure = new DeviceProcedure();

                var groups = treeNodes.Where(_ => _.Data.IsGroup == true);
                if (groups != null && groups.Count() > 0)
                {
                    foreach (var group in groups)
                    {
                        var level2s = treeNodes.Where(_ => _.Data.Group == group.Key && _.Data.IsGroup == false);
                        if (level2s != null && level2s.Count() > 0)
                        {
                            foreach (var level2 in level2s)
                            {
                                var level3s = treeNodes.Where(_ => _.Data.Group == level2.Key && _.Data.IsGroup == false);
                                if (level3s != null && level3s.Count() > 0)
                                {
                                    foreach (var level3 in level3s)
                                    {
                                        deviceProcedure = new DeviceProcedure();

                                        deviceProcedure.DeviceId = deviceId;
                                        deviceProcedure.BodyStructureGroupId = new Guid(group.Key);
                                        deviceProcedure.BodyStructureId = int.Parse(level2.Key);
                                        deviceProcedure.SnomedID = level3.Key.Split('|')[1];
                                        deviceProcedure.UserId = AbpSession.UserId.Value;
                                        deviceProcedure.CreatedDate = DateTime.UtcNow;
                                        deviceProcedure.BodyStructureProcId = null;
                                        //deviceProcedure.HospitalId = hospitalId;

                                        var exists = RowExists(existingDeviceProcedures, deviceProcedure);
                                        if (!exists)
                                        {
                                            var ret = await Save(deviceProcedure, deviceId);
                                            //if (ret)
                                            //    addedDeviceProcedures.Add(deviceProcedure);
                                        }

                                        addedDeviceProcedures.Add(deviceProcedure);

                                        DoneNodes.Add(
                                            treeNodes.FirstOrDefault(_ => _.Key == level3.Key && _.Data.Group == level2.Key && _.Data.IsGroup == false));
                                    };
                                }
                                else
                                {
                                    deviceProcedure = new DeviceProcedure();

                                    deviceProcedure.DeviceId = deviceId;
                                    deviceProcedure.BodyStructureGroupId = new Guid(group.Key);
                                    deviceProcedure.BodyStructureId = int.Parse(level2.Key);
                                    deviceProcedure.BodyStructureProcId = null;
                                    deviceProcedure.UserId = AbpSession.UserId.Value;
                                    deviceProcedure.CreatedDate = DateTime.UtcNow;
                                    deviceProcedure.SnomedID = null;
                                    //deviceProcedure.HospitalId = hospitalId;

                                    var exists = RowExists(existingDeviceProcedures, deviceProcedure);
                                    if (!exists)
                                    {
                                        var ret = await Save(deviceProcedure, deviceId);
                                        //if (ret)
                                        //    addedDeviceProcedures.Add(deviceProcedure);
                                    }
                                    addedDeviceProcedures.Add(deviceProcedure);
                                    DoneNodes.Add(
                                            treeNodes.FirstOrDefault(_ => _.Key == level2.Key && _.Data.Group == group.Key && _.Data.IsGroup == false));
                                }

                            };
                        }
                        else
                        {
                            deviceProcedure = new DeviceProcedure();

                            deviceProcedure.DeviceId = deviceId;
                            deviceProcedure.BodyStructureGroupId = new Guid(group.Key);
                            deviceProcedure.BodyStructureId = null;
                            deviceProcedure.BodyStructureProcId = null;
                            deviceProcedure.UserId = AbpSession.UserId.Value;
                            deviceProcedure.CreatedDate = DateTime.UtcNow;
                            deviceProcedure.SnomedID = null;
                            //deviceProcedure.HospitalId = hospitalId;

                            var exists = RowExists(existingDeviceProcedures, deviceProcedure);
                            if (!exists)
                            {
                                var ret = await Save(deviceProcedure, deviceId);
                                //if (ret)
                                //    addedDeviceProcedures.Add(deviceProcedure);
                            }
                            addedDeviceProcedures.Add(deviceProcedure);
                            DoneNodes.Add(treeNodes.FirstOrDefault(_ => _.Key == group.Key && _.Data.IsGroup == true));
                        }
                    };
                }

                var notYetNodes = treeNodes.Where(_ =>
                        !DoneNodes.Any(d => d.Key == _.Key && d.Data.Group == _.Data.Group) &&
                    _.Data.IsGroup == false
                );

                if (notYetNodes != null && notYetNodes.Count() > 0)
                {
                    foreach (var node in notYetNodes)
                    {
                        var parent = node.Data.Group;
                        Guid guidOut;

                        bool isGuid = Guid.TryParse(parent, out guidOut);
                        if (isGuid)
                        {
                            var group = _bodyStructureGroupsRepository.GetAll()
                                    .Where(_ => _.Id == guidOut).FirstOrDefault();

                            if (group != null)
                            {
                                deviceProcedure = new DeviceProcedure();

                                deviceProcedure.DeviceId = deviceId;
                                deviceProcedure.BodyStructureGroupId = group.Id;
                                deviceProcedure.BodyStructureId = int.Parse(node.Key);
                                deviceProcedure.BodyStructureProcId = null;
                                deviceProcedure.UserId = AbpSession.UserId.Value;
                                deviceProcedure.CreatedDate = DateTime.UtcNow;
                                deviceProcedure.SnomedID = null;
                                //deviceProcedure.HospitalId = hospitalId;

                                var exists = RowExists(existingDeviceProcedures, deviceProcedure);
                                if (!exists)
                                {
                                    var ret = await Save(deviceProcedure, deviceId);
                                    //if (ret)
                                    //    addedDeviceProcedures.Add(deviceProcedure);
                                }
                                addedDeviceProcedures.Add(deviceProcedure);
                            }
                        }
                        else
                        {
                            var childGroup = _bodyStructuresRepository.GetAll()
                                .Where(_ => _.Id == int.Parse(parent)).FirstOrDefault();
                            if (childGroup != null)
                            {
                                var parentOfChild = _bodyStructureGroupsRepository.GetAll()
                                    .Where(_ => _.Id == childGroup.BodyStructureGroupId).FirstOrDefault();

                                if (parentOfChild != null)
                                {
                                    deviceProcedure = new DeviceProcedure();

                                    deviceProcedure.DeviceId = deviceId;
                                    deviceProcedure.BodyStructureGroupId = parentOfChild.Id;
                                    deviceProcedure.BodyStructureId = childGroup.Id;
                                    deviceProcedure.BodyStructureProcId = null;
                                    deviceProcedure.SnomedID = node.Key.Split('|')[1];
                                    deviceProcedure.UserId = AbpSession.UserId.Value;
                                    deviceProcedure.CreatedDate = DateTime.UtcNow;
                                    //deviceProcedure.HospitalId = hospitalId;

                                    var exists = RowExists(existingDeviceProcedures, deviceProcedure);
                                    if (!exists)
                                    {
                                        var ret = await Save(deviceProcedure, deviceId);
                                        //if (ret)
                                        //    addedDeviceProcedures.Add(deviceProcedure);
                                    }
                                    addedDeviceProcedures.Add(deviceProcedure);
                                }
                            }
                        }
                    }
                }

                var soloNodes = treeNodes.Where(_ => !treeNodes.Select(o => o.Key).Contains(_.Data.Group) && _.Data.IsGroup == false);
                if (soloNodes != null && soloNodes.Count() > 0)
                {
                    foreach (var node in soloNodes)
                    {
                        var parent = node.Data.Group;
                        Guid guidOut;

                        bool isGuid = Guid.TryParse(parent, out guidOut);
                        if (isGuid)
                        {
                            var group = _bodyStructureGroupsRepository.GetAll()
                                .Where(_ => _.Id == guidOut).FirstOrDefault();

                            if (group != null)
                            {
                                deviceProcedure = new DeviceProcedure();

                                deviceProcedure.DeviceId = deviceId;
                                deviceProcedure.BodyStructureGroupId = group.Id;
                                deviceProcedure.BodyStructureId = int.Parse(node.Key);
                                deviceProcedure.BodyStructureProcId = null;
                                deviceProcedure.UserId = AbpSession.UserId.Value;
                                deviceProcedure.CreatedDate = DateTime.UtcNow;
                                deviceProcedure.SnomedID = null;
                                //deviceProcedure.HospitalId = hospitalId;

                                var exists = RowExists(existingDeviceProcedures, deviceProcedure);
                                if (!exists)
                                {
                                    var ret = await Save(deviceProcedure, deviceId);
                                    //if (ret)
                                    //    addedDeviceProcedures.Add(deviceProcedure);
                                }
                                addedDeviceProcedures.Add(deviceProcedure);
                            }
                        }
                        else
                        {
                            var childGroup = _bodyStructuresRepository.GetAll()
                                .Where(_ => _.Id == int.Parse(parent)).FirstOrDefault();

                            if (childGroup != null)
                            {
                                var parentOfChild = _bodyStructureGroupsRepository.GetAll()
                                    .Where(_ => _.Id == childGroup.BodyStructureGroupId).FirstOrDefault();

                                if (parentOfChild != null)
                                {
                                    deviceProcedure = new DeviceProcedure();

                                    deviceProcedure.DeviceId = deviceId;
                                    deviceProcedure.BodyStructureGroupId = parentOfChild.Id;
                                    deviceProcedure.BodyStructureId = childGroup.Id;
                                    deviceProcedure.BodyStructureProcId = null;
                                    deviceProcedure.SnomedID = node.Key.Split('|')[1];
                                    deviceProcedure.UserId = AbpSession.UserId.Value;
                                    deviceProcedure.CreatedDate = DateTime.UtcNow;
                                    //deviceProcedure.HospitalId = hospitalId;

                                    var exists = RowExists(existingDeviceProcedures, deviceProcedure);
                                    if (!exists)
                                    {
                                        var ret = await Save(deviceProcedure, deviceId);
                                        //if (ret)
                                        //    addedDeviceProcedures.Add(deviceProcedure);
                                    }
                                    addedDeviceProcedures.Add(deviceProcedure);
                                }
                            }
                        }

                    }
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            else
            {
                if (existingDeviceProcedures != null && existingDeviceProcedures.Count() > 0)
                {
                    foreach (var _ in existingDeviceProcedures)
                    {
                        await _deviceProcedureRepository.DeleteAsync(_);
                    }

                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }

            //await CheckExistingIfDeleted(addedDeviceProcedures, deviceId, hospitalId);
            await CheckExistingIfDeleted(addedDeviceProcedures, deviceId);
        }

        public async Task<IEnumerable<DeviceDto>> GetBySnomedId(string id, int bodyStructureId, string hospitalId, string brandName, int deviceFamilyId, string model)
        {

            if (string.IsNullOrWhiteSpace(id))
                return null;

            var query = await _deviceProcedureRepository.GetAll()
                .Include(_ => _.Device)
                .Include(_ => _.Device.Manufacturer)
                .Include(_ => _.Device.DeviceClass)
                .Include(_ => _.Device.DeviceFamily)
                .Include(_ => _.Device.BodyStructureGroup)
                .Where(_ => _.SnomedID == id && _.BodyStructureId == bodyStructureId)
                .WhereIf(!string.IsNullOrWhiteSpace(brandName), e => e.Device.BrandName.Contains(brandName))
                .WhereIf(deviceFamilyId > 0, e => e.Device.DeviceFamilyId == deviceFamilyId)
                .WhereIf(!string.IsNullOrWhiteSpace(model), e => e.Device.Model.ToLower().Contains(model.ToLower()))
                .ToListAsync();

            IList<DeviceDto> deviceDtos = new List<DeviceDto>();

            query = query.Where(_ => 
                _deviceHospitalRepository.GetAll().Any(d => _.DeviceId == d.DeviceId && d.HospitalId == hospitalId)
            ).ToList();

            if (query != null && query.Count() > 0)
            {

                foreach (var proc in query)
                {
                    DeviceDto device = new DeviceDto();

                    if (proc != null)
                    {
                        device.Id = proc.Device.Id;
                        device.DeviceName = proc.Device.DeviceName;
                        device.UID = proc.Device.UID;
                        device.DeviceDescription = proc.Device.DeviceDescription;
                        device.GMDNTermCode = proc.Device.GMDNTermCode;
                        device.BrandName = proc.Device.BrandName;
                        device.Model = proc.Device.Model;
                        device.DocFileId = proc.Device.DocFileId;
                        device.Status = proc.Device.Status;
                        device.ManufacturerId = proc.Device.ManufacturerId;
                        device.UserId = proc.Device.UserId;
                        device.CreatedDate = proc.Device.CreatedDate;
                        device.ManufacturerName = proc.Device.Manufacturer.Name;
                        device.Manufacturer = ObjectMapper.Map<Manufactures.Dto.ManufactureDto>(proc.Device.Manufacturer);
                        device.DeviceClassName = proc.Device.DeviceClass != null ? "Class " + proc.Device.DeviceClass.Class : "";
                        device.DeviceClass = proc.Device.DeviceClass != null ? ObjectMapper.Map<DeviceClassDto>(proc.Device.DeviceClass) : null;
                        device.DeviceFamilyId = proc.Device.Id;
                        device.DeviceFamily = proc.Device.DeviceFamily != null ? ObjectMapper.Map<DeviceFamilyDto>(proc.Device.DeviceFamily) : null;
                        device.SpecialtyName = proc.Device.BodyStructureGroup != null ? proc.Device.BodyStructureGroup.Name : "";

                        deviceDtos.Add(device);
                    }
                }
            }

            if (deviceDtos.Count > 0)
                deviceDtos = deviceDtos.GroupBy(_ => _.Id).Select(_ => _.FirstOrDefault()).ToList();

            return deviceDtos;
        }

        /// <summary>
        /// Grouped by Brand and DeviceFamily
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bodyStructureId"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<DeviceBrandFamilyViewDto> GetBySnomedIdGrouped(string id, int bodyStructureId, string hospitalId, string bodyStructureGroupId, bool isFilterLicensedStatus)
        {
            DeviceBrandFamilyViewDto deviceBrandFamilyViewDto = new DeviceBrandFamilyViewDto()
            {
                IsLicensed = false
            };
            if (string.IsNullOrWhiteSpace(id))
                return deviceBrandFamilyViewDto;

            if (isFilterLicensedStatus) //filter only when user is a surgeon|admin
            {
                var isBodyStructGrpLicensed = await _specialtyInfoRepository.GetAll().AnyAsync(_ => _.HospitalId == hospitalId && _.SpecialtyId.ToString() == bodyStructureGroupId);
                if (!isBodyStructGrpLicensed)
                {
                    return deviceBrandFamilyViewDto;
                }
            }
            else
            {
                deviceBrandFamilyViewDto.IsLicensed = true;
            }

            var query = await _deviceProcedureRepository.GetAll()
                .Include(_ => _.Device)
                .Include(_ => _.Device.Manufacturer)
                .Include(_ => _.Device.DeviceClass)
                .Include(_ => _.Device.DeviceFamily)
                .Include(_ => _.Device.BodyStructureGroup)                
                .Where(_ => _.SnomedID == id && _.BodyStructureId == bodyStructureId)
                .ToListAsync();

            IList<DeviceDto> deviceDtos = new List<DeviceDto>();

            query = query.Where(_ =>
                _deviceHospitalRepository.GetAll().Any(d => _.DeviceId == d.DeviceId && d.HospitalId == hospitalId)
            ).ToList();

            if (query != null && query.Count() > 0)
            {

                foreach (var proc in query)
                {
                    DeviceDto device = new DeviceDto();

                    if (proc != null)
                    {
                        device.Id = proc.Device.Id;
                        device.DeviceName = proc.Device.DeviceName;
                        device.UID = proc.Device.UID;
                        device.DeviceDescription = proc.Device.DeviceDescription;
                        device.GMDNTermCode = proc.Device.GMDNTermCode;
                        device.BrandName = proc.Device.BrandName;
                        device.Model = proc.Device.Model;
                        device.DocFileId = proc.Device.DocFileId;
                        device.Status = proc.Device.Status;
                        device.ManufacturerId = proc.Device.ManufacturerId;
                        device.UserId = proc.Device.UserId;
                        device.CreatedDate = proc.Device.CreatedDate;
                        device.ManufacturerName = proc.Device.Manufacturer.Name;
                        device.Manufacturer = ObjectMapper.Map<Manufactures.Dto.ManufactureDto>(proc.Device.Manufacturer);
                        device.DeviceClassName = proc.Device.DeviceClass != null ? "Class " + proc.Device.DeviceClass.Class : "";
                        device.DeviceClass = proc.Device.DeviceClass != null ? ObjectMapper.Map<DeviceClassDto>(proc.Device.DeviceClass) : null;
                        device.DeviceFamilyId = proc.Device.DeviceFamilyId;
                        device.DeviceFamily = proc.Device.DeviceFamily != null ? ObjectMapper.Map<DeviceFamilyDto>(proc.Device.DeviceFamily) : null;
                        device.SpecialtyName = proc.Device.BodyStructureGroup != null ? proc.Device.BodyStructureGroup.Name : "";

                        deviceDtos.Add(device);
                    }
                }
            }

            IList<DeviceBrandFamilyDto> filteredDevices = new List<DeviceBrandFamilyDto>();

            if (deviceDtos.Count > 0)
            {
                filteredDevices = deviceDtos.GroupBy(_ =>
                        new
                        {
                            _.BrandName,
                            _.DeviceFamilyId,
                            DeviceFamilyName = _.DeviceFamily.Name,
                            _.ManufacturerId,
                            ManufacturerName = _.Manufacturer.Name,
                            UniqueID = _.BrandName + "|__|" + _.DeviceFamilyId
                        })
                        .Select(_ => new DeviceBrandFamilyDto()
                        {
                            BrandName = _.Key.BrandName,
                            DeviceFamilyId = _.Key.DeviceFamilyId,
                            DeviceFamilyName = _.Key.DeviceFamilyName,
                            ManufacturerId = _.Key.ManufacturerId,
                            ManufacturerName = _.Key.ManufacturerName,
                            UniqueID = _.Key.UniqueID
                        }).ToList();
            }

            deviceBrandFamilyViewDto.DeviceBrandFamilies = filteredDevices;
            deviceBrandFamilyViewDto.IsLicensed = true;

            return deviceBrandFamilyViewDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceDtos"></param>
        /// <param name="bodyStructureId"></param>
        /// <returns></returns>
        private async Task<List<DeviceDto>> GetSurgicalProcedures(List<DeviceDto> deviceDtos, int bodyStructureId, string id)
        {
            using (Snowstorm snomedApi = new Snowstorm(new Uri(_snomedApiConfiguration.BaseUrl)))
            {
                var conceptResult = await snomedApi.FindConceptsUsingGETMethodAsync(_snomedApiConfiguration.Branch, ecl: await GetSnomedQuery(bodyStructureId), limit: 1000);
                if (conceptResult != null && conceptResult.Items != null)
                {
                    var conceptResultItems = (IList)conceptResult.Items;
                    var exempted = string.Join(",", Enum.GetValues(typeof(ExemptedBodyStructures)).Cast<int>()).Split(",");
                    var isExempted = exempted.Any(e => e == bodyStructureId.ToString());

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
                                    if (concept.ConceptId == id)
                                    {
                                        var query = await _deviceProcedureRepository.GetAll()
                                        .Include(_ => _.Device)
                                        .Include(_ => _.BodyStructureSubProcedure)
                                        .Where(_ => _.BodyStructureId == bodyStructureId).ToListAsync();

                                        if (query != null && query.Count() > 0)
                                        {
                                            foreach (var proc in query)
                                            {
                                                DeviceDto device = new DeviceDto();

                                                if (proc != null)
                                                {
                                                    device.Id = proc.Device.Id;
                                                    device.DeviceName = proc.Device.DeviceName;
                                                    device.UID = proc.Device.UID;
                                                    device.DeviceDescription = proc.Device.DeviceDescription;
                                                    device.GMDNTermCode = proc.Device.GMDNTermCode;
                                                    device.BrandName = proc.Device.BrandName;
                                                    device.Model = proc.Device.Model;
                                                    device.DocFileId = proc.Device.DocFileId;
                                                    device.Status = proc.Device.Status;
                                                    device.ManufacturerId = proc.Device.ManufacturerId;
                                                    device.UserId = proc.Device.UserId;
                                                    device.CreatedDate = proc.Device.CreatedDate;

                                                    var isExist = deviceDtos.Where(_ => _ == device).Any();
                                                    if (!isExist)
                                                        deviceDtos.Add(device);
                                                }
                                            }
                                        }
                                    }
                                }


                            }
                        }

                    }

                    var subprocedures = await _bodyStructureSubProcedureRepository.GetAll()
                                        .Where(e => e.BodyStructureId == bodyStructureId)
                                        .ToListAsync();

                    if (subprocedures.Count > 0)
                    {
                        foreach (var subprocedureItem in subprocedures)
                        {
                            var query = await _deviceProcedureRepository.GetAll()
                                .Include(_ => _.Device)
                                .Include(_ => _.BodyStructureSubProcedure)
                                .Where(_ => _.BodyStructureSubProcedure.SnomedId == subprocedureItem.SnomedId).ToListAsync();

                            if (query != null && query.Count() > 0)
                            {
                                foreach (var proc in query)
                                {
                                    DeviceDto device = new DeviceDto();

                                    if (proc != null)
                                    {
                                        device.Id = proc.Device.Id;
                                        device.DeviceName = proc.Device.DeviceName;
                                        device.UID = proc.Device.UID;
                                        device.DeviceDescription = proc.Device.DeviceDescription;
                                        device.GMDNTermCode = proc.Device.GMDNTermCode;
                                        device.BrandName = proc.Device.BrandName;
                                        device.Model = proc.Device.Model;
                                        device.DocFileId = proc.Device.DocFileId;
                                        device.Status = proc.Device.Status;
                                        device.ManufacturerId = proc.Device.ManufacturerId;
                                        device.UserId = proc.Device.UserId;
                                        device.CreatedDate = proc.Device.CreatedDate;

                                        var isExist = deviceDtos.Where(_ => _ == device).Any();
                                        if (!isExist)
                                            deviceDtos.Add(device);
                                    }
                                }
                            }
                        }
                    }
                }

                return deviceDtos;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceProcedure"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private async Task<bool> Save(DeviceProcedure deviceProcedure, int deviceId)
        {
            await _deviceProcedureRepository.InsertAsync(deviceProcedure);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceProcedures"></param>
        /// <returns></returns>
        private async Task CheckExistingIfDeleted(IList<DeviceProcedure> deviceProcedures, int deviceId)
        {
            try
            {
                var deleted = _deviceProcedureRepository.GetAll().AsEnumerable()
                    .Where(_ => !deviceProcedures.Any(
                        dp => dp.BodyStructureGroupId == _.BodyStructureGroupId &&
                        dp.BodyStructureId == _.BodyStructureId && dp.SnomedID == _.SnomedID) &&
                        _.DeviceId == deviceId).ToList();

                if (deleted != null)
                {
                    if (deleted.Count() > 0)
                    {
                        foreach (var _ in deleted)
                        {
                            await _deviceProcedureRepository.DeleteAsync(_);
                        }

                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool RowExists(List<DeviceProcedure> existingDeviceProcedures, DeviceProcedure forAdd)
        {
            var query = existingDeviceProcedures
                .Any(_ => _.BodyStructureGroupId == forAdd.BodyStructureGroupId &&
                _.BodyStructureId == forAdd.BodyStructureId && _.SnomedID == forAdd.SnomedID
                && _.DeviceId == forAdd.DeviceId && _.HospitalId == forAdd.HospitalId);

            return query;
        }
    }
}
