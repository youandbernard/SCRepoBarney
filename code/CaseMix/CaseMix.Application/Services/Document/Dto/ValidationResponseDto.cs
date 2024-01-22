using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Document.Dto
{
    public class ValidationResponseDto
    {
        public int NumberRows { get; set; } = 0;
        public bool HasErrors { get; set; } = false;
        public List<FileErrorDto> Errors { get; set; }
    }
}
