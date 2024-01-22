using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Trusts.Dto
{
    [AutoMap(typeof(TrustEntity))]
    public class TrustDto : EntityDto<int>
    {
        public string Region { get; set; }
        public string Trust { get; set; }
        public string GroupTrust { get; set; }
        public string TrustWebsite { get; set; }
        public string InnovationTeamContactName { get; set; }
        public string InnovationTeamWebsite { get; set; }
        public string ContactRole { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public bool Contacted { get; set; }
    }
}
