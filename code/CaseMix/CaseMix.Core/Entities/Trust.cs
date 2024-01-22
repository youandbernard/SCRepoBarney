using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("Trusts")]
    public class TrustEntity: Entity<int>
    {
        public string Region { get; set; }
        public string Ics { get; set; }
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
