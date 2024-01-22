using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Document.Dto
{
    public class FileErrorDto
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public int Line { get; set; }
        public string Message { get; set; }

    }
}
