using CaseMix.Aws.OpenSearch.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Aws.OpenSearch.Services
{
    public interface IOpenSearchService
    {
        Task<OpenSearchResponseDto> UploadAsync(string endpoint, string masterUser, string masterPassword, string data);
    }
}
