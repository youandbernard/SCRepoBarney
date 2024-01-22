using Abp.Zero.EntityFrameworkCore;
using CaseMix.Authorization.Roles;
using CaseMix.Authorization.Users;
using CaseMix.Entities;
using CaseMix.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace CaseMix.EntityFrameworkCore
{
    public class CaseMixDbContext : AbpZeroDbContext<Tenant, Role, User, CaseMixDbContext>
    {
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<UserHospital> UserHospitals { get; set; }
        public virtual DbSet<PasswordReset> PasswordResets { get; set; }
        public virtual DbSet<BodyStructure> BodyStructures { get; set; }
        public virtual DbSet<PatientSurgeryProgress> PatientSurgeryProgresses { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<HospitalPatient> HospitalPatients { get; set; }
        public virtual DbSet<PreOperativeAssessment> PreOperativeAssessments { get; set; }
        public virtual DbSet<PoapProcedure> PoapProcedures { get; set; }
        public virtual DbSet<PoapRisk> PoapRisks { get; set; }
        public virtual DbSet<PatientSurvey> PatientSurveys { get; set; }
        public virtual DbSet<PatientSurveyNotes> PatientSurveysNotes { get; set; }
        public virtual DbSet<BodyStructureGroup> BodyStructureGroups { get; set; }
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }
        public virtual DbSet<ComorbidityGroup> ComorbidityGroups { get; set; }
        public virtual DbSet<Comorbidity> Comorbidities { get; set; }
        public virtual DbSet<HospitalSpecialty> HospitalSpecialties { get; set; }
        public virtual DbSet<SurgeonSpecialty> SurgeonSpecialties { get; set; }
        public virtual DbSet<Theater> Theaters { get; set; }
        public virtual DbSet<BodyStructureSubProcedure> BodyStructureSubProcedures { get; set; }
        public virtual DbSet<SurveyTimestampSetting> SurveyTimestampSettings { get; set; }
        public virtual DbSet<ReportingSetting> ReportingSettings { get; set; }
        public virtual DbSet<UserDisplayCompletedSurveySetting> UserDisplayCompletedSurveySettings { get; set; }
        public virtual DbSet<ProcedureMethodType> ProcedureMethodTypes { get; set; }
        public virtual DbSet<PoapProcedureMethodType> PoapProcedureMethodTypes { get; set; }
        public virtual DbSet<RiskMappingSetting> RiskMappingSettings { get; set; }
        public virtual DbSet<DiagnosticReport> DiagnosticReport { get; set; }
        public virtual DbSet<PoapRiskFactor> PoapRiskFactors { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Manufacture> Manufactures { get; set; }
        public virtual DbSet<UserRealmMapping> UserRealmMappings { get; set; }
        public virtual DbSet<TrustEntity> Trusts { get; set; }
        public virtual DbSet<UkRegion> UkRegions { get; set; }
        public virtual DbSet<IntegratedCareSystem> IntegratedCareSystems { get; set; }
        public virtual DbSet<TrustIcsMapping> TrustIcsMappings { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DocumentFile> DocumentFiles { get; set; }
        public virtual DbSet<DeviceProcedure> DeviceProcedures { get; set; }
        public virtual DbSet<PoapProcedureDevices> PoapProcedureDevices { get; set; }
        public virtual DbSet<GmdnAgencies> GmdnAgencies { get; set; }
        public virtual DbSet<Instrument> Instruments { get; set; }
        public virtual DbSet<InstrumentPack> InstrumentPacks { get; set; }
        public virtual DbSet<PoapInstrumentPack> PoapInstrumentPacks { get; set; }
        public virtual DbSet<DeviceClass> DeviceClass { get; set; }
        public virtual DbSet<DeviceFamily> DeviceFamily { get; set; }
        public virtual DbSet<DevicesHospital> DevicesHospital { get; set; }
        public virtual DbSet<SpecialtyInfo> SpecialtyInfo { get; set; }

        public CaseMixDbContext(DbContextOptions<CaseMixDbContext> options)
            : base(options)
        {
        }
    }
}
