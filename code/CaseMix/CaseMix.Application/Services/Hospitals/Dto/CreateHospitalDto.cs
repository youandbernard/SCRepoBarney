using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Hospitals.Dto
{
    public class CreateHospitalDto
    {
        public Guid? CountryId { get; set; }
        public Guid? RegionId { get; set; }
        public Guid? TrustId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Postcode { get; set; }
    }
}
