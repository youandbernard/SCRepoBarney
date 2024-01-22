using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Document.Dto
{
    public class DownloadFileDto
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
    }
}
