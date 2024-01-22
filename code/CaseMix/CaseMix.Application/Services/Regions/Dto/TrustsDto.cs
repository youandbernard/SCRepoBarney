using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Regions.Dto
{
    public class TrustsDto
    {
        public int? TrustId { get; set; }
        public Guid? RegionId { get; set; }
        public string RegionName { get; set; }
        public string GroupTrust { get; set; }
        public string Type { get; set; }
        public Guid? ParentId { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
