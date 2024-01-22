using Abp.Application.Services.Dto;
using CaseMix.Entities.Enums;
using System;
using System.Collections.Generic;

namespace CaseMix.Services.PreOperativeAssessments.Dto
{
    public class CreateUpdatePreOperativeAssessmentDto : EntityDto<Guid>
    {
        public string HospitalId { get; set; }

        public string PatientId { get; set; }
        public int DateOfBirthYear { get; set; }
        public GenderType Gender { get; set; }
        public DateTime AssessmentDate { get; set; }
        public DateTime SurgeryDate { get; set; }

        public long SurgeonId { get; set; }
        public string SurgeonName { get; set; }
        public SurgeonExperienceType SurgeonExperience { get; set; }
        public string AnesthetistName { get; set; }
        public Guid TheaterId { get; set; }

        public IEnumerable<PoapProcedureDto> Procedures { get; set; }
        public IEnumerable<PoapRiskDto> Risks { get; set; }
    }
}
