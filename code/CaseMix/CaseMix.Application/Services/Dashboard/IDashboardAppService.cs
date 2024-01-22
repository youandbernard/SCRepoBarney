using CaseMix.Aws.OpenSearch.Model;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.Dashboard
{
    public interface IDashboardAppService
    {
        Task<OpenSearchResponseDto> UploadHospitalAsync(Hospital hospital);
    }
}
