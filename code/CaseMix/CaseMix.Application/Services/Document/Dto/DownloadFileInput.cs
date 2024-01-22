using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Document.Dto
{
    public class DownloadFileInput
    {
        public long UserId { get; set; }
        public int DocumentId { get; set; }
    }
}
