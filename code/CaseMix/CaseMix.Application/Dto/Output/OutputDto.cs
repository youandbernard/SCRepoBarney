using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Dto.Output
{
    public class OutputDto
    {
        public bool IsSuccess { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
