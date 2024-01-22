using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Document.Dto
{
    public class PagedDocumentFileResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
