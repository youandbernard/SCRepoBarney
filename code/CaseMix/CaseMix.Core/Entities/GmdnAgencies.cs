using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Entities
{
    [Table("gmdnagencies")]
    public class GmdnAgencies : Entity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string TermIsIVD { get; set; }
        public string ModifiedDate { get; set; }
        public string ObsoletedDate { get; set; }
    }
}
