using Abp.Application.Services.Dto;
using CaseMix.Entities.Enums;
using System;

namespace CaseMix.Services.Patients.Dto
{
    public class PatientDto : EntityDto<string>
    {
        public DateTime? DateOfBirth { get; set; }
        public GenderType? Gender { get; set; }
        public int? DobYear { get; set; }
        public string Name { get; set; }
        public bool Deceased { get; set; }
        public string Address { get; set; }
        public string EthnicCategory { get; set; }
    }
}
