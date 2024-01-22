using AutoMapper;
using CaseMix.Dto;
using CaseMix.Entities;
using CaseMix.Services.DiagnosticReports.Dto;
using CaseMix.Services.Hospitals.Dto;
using CaseMix.Services.Patients.Dto;
using CaseMix.Services.PreOperativeAssessments.Dto;
using CaseMix.Services.RiskMappingSettings.Dto;
using CaseMix.Services.SurveyTimeStamps.Dto;
using CaseMix.Services.Theaters.Dto;
using SnomedApi.Models;

namespace CaseMix
{
    public class CaseMixMapperProfile : Profile
    {
        public CaseMixMapperProfile()
        {
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.DobYear, opt => { 
                    opt.PreCondition(src => src.DateOfBirth.HasValue);
                    opt.MapFrom(src => src.DateOfBirth.Value.Year);
                });
            CreateMap<CreateUpdatePreOperativeAssessmentDto, PreOperativeAssessment>();
            CreateMap<SurveyTimestampSettingDto, SurveyTimestampSetting>();
            CreateMap<RiskMappingSettingDto, RiskMappingSetting>();
            CreateMap<DiagnosticReportDto, DiagnosticReport>();
            CreateMap<HospitalDto, Hospital>();
            CreateMap<BodyStructure, BodyStructureOutputDto>()
                .ForMember(dest => dest.snomedId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.snomed_desc, opt => opt.MapFrom(src => src.Description));
            CreateMap<Concept, SurgicalProcedureOutputDto>()
                .ForMember(dest => dest.snomedId, opt => opt.MapFrom(src => src.ConceptId))
                .ForMember(dest => dest.snomed_desc, opt => opt.MapFrom(src => src.Fsn.Term));
            CreateMap<Theater, SearchTheaterDto>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => $"{src.TheaterId} / {src.Name}"));
        }
    }
}
