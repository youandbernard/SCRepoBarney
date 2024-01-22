using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Aws.OpenSearch.Model
{
    public class OpenSearchResponseDto
    {
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public List<string> Errors { get; set; }
    }
}
