using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Authorization.Users;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Services.BodyStructures.Dto;
using CaseMix.Services.DiagnosticReports.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Patients.Dto;
using CaseMix.Services.PoapInstrumentPacks.Dto;
using CaseMix.Services.Theaters.Dto;
using CaseMix.Users.Dto;
using System;
using System.Collections.Generic;

namespace CaseMix.Services.PreOperativeAssessments.Dto
{
    [AutoMap(typeof(PreOperativeAssessment))]
    public class PreOperativeAssessmentDto : EntityDto<Guid>
    {
        public string SurgeonName { get; set; }
        public SurgeonExperienceType? SurgeonExperience { get; set; }
        public string AnesthetistName { get; set; }
        public int? DateOfBirthYear { get; set; }
        public GenderType? Gender { get; set; }
        public StatusType? Status { get; set; }
        public DateTime AssessmentDate { get; set; }
        public DateTime SurgeryDate { get; set; }
        public Guid TheaterId { get; set; }
        public string MethodId { get; set; }
        public string HospitalId { get; set; }
        public string PatientId { get; set; }
        public long SurgeonId { get; set; }
        public int BodyStructureId { get; set; }
        public double TotalMeanTime { get; set; }

        public int EthnicityId { get; set; }

        public bool IsSmoker { get; set; }
        public bool IsSelectAll { get; set; }
        public bool IsArchived { get; set; }
        public TimeSpan? ClockStartTimestamp { get; set; }
        public TimeSpan? ClockEndTimeStamp { get; set; }
        public Guid? BodyStructureGroupId { get; set; }
        public string Timezone { get; set; }
        public IEnumerable<ProcedureMethodTypeDto> ProcedureMethodTypes { get; set; }
        public IEnumerable<InstrumentPackDto> InstrumentPacks { get; set; }
        public IEnumerable<PoapProcedureDto> Procedures { get; set; }
        public IEnumerable<PoapRiskDto> Risks { get; set; }
        public int AwaitingRiskCompletion { get; set; }
        public int? DiagnosticReportId { get; set; }

        public BodyStructureDto BodyStructure { get; set; }
        public PatientDto Patient { get; set; }
        public UserDto Surgeon { get; set; }
        public HospitalDto Hospital { get; set; }
        public TheaterDto Theater { get; set; }
        public DiagnosticReportDto DiagnosticReport { get; set; }
    }
}
