using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Services.BodyStructureGroups.Dto
{
    [AutoMap(typeof(SpecialtyInfo))]
    public class SpecialtyInfoDto: EntityDto<int>
    {
        public Guid SpecialtyId { get; set; }

        public string HospitalId { get; set; }

        public string LicenseDesc { get; set; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [NotMapped]
        public string SpecialtyName { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
