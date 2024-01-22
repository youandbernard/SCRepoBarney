using Abp.Application.Services;
using CaseMix.Services.Device.Dto;
using CaseMix.Services.Document.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.Document
{
    public interface IDocumentAppService: IAsyncCrudAppService<DocumentFileDto, int, PagedDocumentFileResultRequestDto>
    {
        Task EnableOrDisableAsync(int id, bool flag);
        Task<DownloadFileDto> DownloadFile(DownloadFileInput input);
        ValidationResponseDto BasicValidation([FromForm] FileDto fileInput);
        ValidationResponseDto ValidateDataFiles([FromForm] FileDto fileInput);
    }
}
