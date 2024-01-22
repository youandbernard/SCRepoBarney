using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using CaseMix.Aws.S3.Services;
using CaseMix.Core.Shared.Files;
using CaseMix.Entities;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.Document.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.Document
{
    public class DocumentAppService : AsyncCrudAppService<DocumentFile, DocumentFileDto, int, PagedDocumentFileResultRequestDto>, IDocumentAppService
    {
        private readonly IRepository<Manufacture, Guid> _manufacturesRepository;
        private readonly IRepository<CaseMix.Entities.Device, int> _deviceRepository;
        private readonly UserManager _userManager;
        private readonly IS3Service _s3Service;

        private string[] _uploadHeaders = new string[] { "udi", "gmdntermcode", "devicename", "devicedescription", "brandname", "model", "specialty", "devicefamily", "class", "gtincode" };

        public DocumentAppService(
            IS3Service s3Service,
            UserManager userManager,
            IRepository<Manufacture, Guid> manufacturesRepository,
            IRepository<DocumentFile, int> documentRepository,
            IRepository<CaseMix.Entities.Device, int> deviceRepository) : base(documentRepository)
        {
            _s3Service = s3Service;
            _manufacturesRepository = manufacturesRepository;
            _deviceRepository = deviceRepository;
            _userManager = userManager;

            LocalizationSourceName = CaseMixConsts.LocalizationSourceName;
        }

        protected override IQueryable<DocumentFile> CreateFilteredQuery(PagedDocumentFileResultRequestDto input)
        {
            try
            {
                var searchKey = string.IsNullOrEmpty(input.Keyword) ? string.Empty : input.Keyword.Trim().ToLower();
                var currentUser = _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
                var currentRoles = _userManager.GetRolesAsync(currentUser.Result);
                var isManufacturer = currentRoles.Result.Contains(StaticRoleNames.Tenants.Manufacturer);
                var devicesData = _deviceRepository.GetAll()
                                                   .Include(i => i.Manufacturer)
                                                   .Include(i => i.BodyStructureGroup)
                                                   .Include(i => i.DeviceFamily)
                                                   .Include(i => i.DeviceClass).DefaultIfEmpty().Select(s => new DeviceDocDto
                                                   {
                                                       SpecialtyName = s.BodyStructureGroup.Name ?? string.Empty,
                                                       DeviceFamilyName = s.DeviceFamily.Name ?? string.Empty,
                                                       DeviceClassName = s.DeviceClass.Class ?? string.Empty,
                                                       Manufacturer = s.Manufacturer.Name ?? string.Empty,
                                                       DocFileId = s.DocFileId
                                                   }).Distinct();

                var ret = base.CreateFilteredQuery(input)
                    .Join(devicesData, doc => doc.Id, device => device.DocFileId,
                        (doc, device) => new
                        {
                            doc,
                            device
                        })
                    .Where(_ => _.doc.Active == true)
                    .Where(_ => (isManufacturer && _.doc.ManufacturerId == currentUser.Result.ManufactureId)
                                || !isManufacturer)
                    .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                            _ => _.doc.Filename.ToLower().Contains(searchKey) ||
                            _.doc.Id.ToString().Contains(searchKey) ||
                            _.doc.Filepath.ToLower().Contains(searchKey) ||
                            _.doc.Filetype.ToLower().Contains(searchKey) ||
                            _.doc.Active.ToString().Contains(searchKey) ||
                            _.device.SpecialtyName.ToLower().Contains(searchKey) ||
                            _.device.DeviceClassName.ToLower().Contains(searchKey) ||
                            _.device.DeviceFamilyName.ToLower().Contains(searchKey) ||
                            _.device.Manufacturer.ToLower().Contains(searchKey) ||
                            _.doc.DateUploaded.ToString().Contains(searchKey))
                    .Select(s => new DocumentFile
                    {
                        Id = s.doc.Id,
                        Filename = s.doc.Filename,
                        Filepath = s.doc.Filepath,
                        ManufacturerId = s.doc.ManufacturerId,
                        Manufacturer = s.device.Manufacturer,
                        Enable = s.doc.Enable,
                        Active = s.doc.Active,
                        DateUploaded = s.doc.DateUploaded,
                        SpecialtyName = s.device.SpecialtyName,
                        DeviceFamilyName = s.device.DeviceFamilyName,
                        DeviceClassName = s.device.DeviceClassName
                    });

                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override async Task<DocumentFileDto> GetAsync(EntityDto<int> input)
        {
            var output = await Repository.GetAll()
                .Where(_ => _.Id == input.Id)
                .Select(_ => ObjectMapper.Map<DocumentFileDto>(_))
                .FirstOrDefaultAsync();

            return output;
        }

        public async Task EnableOrDisableAsync(int id, bool flag)
        {
            var document = await Repository.GetAsync(id);
            if (document != null)
            {
                document.Enable = flag;

                await Repository.UpdateAsync(document);
            }
        }

        public async Task<DownloadFileDto> DownloadFile(DownloadFileInput input)
        {
            var document = await Repository.FirstOrDefaultAsync(d => d.Id == input.DocumentId);
            var splittedFilename = document.Filename.ToString().Split('/', StringSplitOptions.RemoveEmptyEntries);
            var fileName = splittedFilename.Last();

            var file = await _s3Service.DownloadAsync(document.Filename, null, 0);

            return new DownloadFileDto
            {
                FileName = fileName,
                FileType = document.Filetype,
                FileContent = file
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        public ValidationResponseDto BasicValidation([FromForm] FileDto fileInput)
        {
            ValidationResponseDto response = new ValidationResponseDto();
            response.Errors = new List<FileErrorDto>();

            FileErrorDto errorDto = new FileErrorDto();
            var file = fileInput.File;

            try
            {
                if (file.ContentType.ToString().ToLower() != "text/csv" && file.ContentType.ToString().ToLower() != "application/vnd.ms-excel")
                {

                    errorDto = new FileErrorDto();

                    errorDto.Message = L("FileShouldCSV");
                    response.HasErrors = true;

                    response.Errors.Add(errorDto);
                }
                double fileLen = (file.Length / 1024f) / 1000;
                if (fileLen > 2)
                {
                    errorDto = new FileErrorDto();
                    errorDto.Message = L("NotExceed2MB");
                    response.HasErrors = true;

                    response.Errors.Add(errorDto);
                }

                //No need to parse if not a CSV file
                if (response.Errors.Count <= 0)
                {
                    var dt = FileHelper.CSVtoDataTableParser(file);
                    if (dt != null)
                    {
                        string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToLower().Trim()).ToArray();
                        if (columnNames.Length != _uploadHeaders.Length)
                        {
                            errorDto = new FileErrorDto();
                            errorDto.Message = L("IncorrectNumberOfFields");
                            response.HasErrors = true;

                            response.Errors.Add(errorDto);
                        }
                        else
                        {
                            var eq = _uploadHeaders.SequenceEqual(columnNames);
                            if (!eq)
                            {
                                errorDto = new FileErrorDto();
                                errorDto.Message = L("HeadersNotMatchedToTemplate");
                                response.HasErrors = true;

                                response.Errors.Add(errorDto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorDto = new FileErrorDto();
                errorDto.Message = "Invalid file or headers not matched to template.";
                response.HasErrors = true;

                response.Errors.Add(errorDto);
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        public ValidationResponseDto ValidateDataFiles([FromForm] FileDto fileInput)
        {
            ValidationResponseDto response = new ValidationResponseDto();
            response.Errors = new List<FileErrorDto>();
            FileErrorDto errorDto = new FileErrorDto();

            var file = fileInput.File;

            List<string> errors;
            int numRows;

            var dt = FileHelper.CSVtoDataTableParserWithValidation(file, fileInput.BodyStructureGroupId, fileInput.DeviceClassId, fileInput.DeviceFamilyId.Value, out errors, out numRows);
            if (dt != null)
            {
                if (errors.Count > 0)
                {
                    response.HasErrors = true;

                    foreach (var _ in errors)
                    {
                        errorDto = new FileErrorDto();

                        errorDto.Message = _.ToString();
                        response.Errors.Add(errorDto);
                    }
                }
                else
                {
                    if (numRows > 0)
                    {
                        response.HasErrors = false;
                        response.NumberRows = numRows;
                    }
                    else
                    {
                        errorDto = new FileErrorDto();

                        response.HasErrors = true;
                        errorDto.Message = L("CSVFileEmpty");

                        response.Errors.Add(errorDto);
                    }

                }
            }
            else
            {
                errorDto = new FileErrorDto();

                response.HasErrors = true;
                errorDto.Message = L("InvalidCSVFile");

                response.Errors.Add(errorDto);
            }

            return response;
        }

    }
}
