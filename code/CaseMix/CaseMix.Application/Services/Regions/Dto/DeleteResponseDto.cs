using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Regions.Dto
{
    public class DeleteResponseDto
    {
        public bool Deleted { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}
