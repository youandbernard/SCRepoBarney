using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using CaseMix.Entities.Enums;
using CaseMix.Services.BodyStructures.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using CaseMix.Services.Theaters.Dto;
using CaseMix.Users.Dto;
using System;
using System.Collections.Generic;

namespace CaseMix.Services.PatientSurveys.Dto
{
    [AutoMap(typeof(PatientSurvey))]
    public class PatientSurveyDto : EntityDto<Guid>
    {
        public string PatientId { get; set; }
        public string HospitalId { get; set; }
        public int BodyStructureId { get; set; }
        public long SurgeonId { get; set; }
        public Guid PreOperativeAssessmentId { get; set; }

        public int? PatientDobYear { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public string EndTime { get; set; }
        public string NextLocation { get; set; }
        public Guid TheaterId { get; set; }
        public string MethodId { get; set; }
        public string ObserverNotes { get; set; }
        public string ObserverStaffId { get; set; }
        public string ObserverSignature { get; set; }
        public double TotalMeanTime { get; set; }
        public string WeekOfYear { get; set; }
        public string StartTime { get; set; } 
        public DateTime? DateStart { get; set; }

        public BodyStructureDto BodyStructure { get; set; }
        public UserDto Surgeon { get; set; }
        public HospitalDto Hospital { get; set; }
        public PreOperativeAssessmentDto PreOperativeAssessment { get; set; }
        public TheaterDto Theater { get; set; }
        public bool isAdmin { get; set; }
        public bool IsReplicate { get; set; }
        public PatientSurveyStatus Status { get; set; }
        public IEnumerable<PatientSurveyNotesDto> PatientSurveyNotes { get; set; }
    }
}
