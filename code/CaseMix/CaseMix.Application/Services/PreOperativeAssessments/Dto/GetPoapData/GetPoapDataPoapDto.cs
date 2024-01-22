using System;
using System.Collections.Generic;

namespace CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData
{
    public class GetPoapDataPoapDto
    {
        public Guid PoapId { get; set; }
        public string HospitalId { get; set; }
        public string TheatreId { get; set; }
        public string PatientId { get; set; }
        public int PatientDOBYear { get; set; }
        public DateTime AssessmentDate { get; set; }
        public DateTime SurgeryDate { get; set; }
        public string Gender { get; set; }
        public string AnesthetistName { get; set; }
        public string Ethnicity { get; set; }
        public double TotalMeanTime { get; set; }
        public double TotalStandardDeviation { get; set; }

        public IEnumerable<GetPoapDataRiskDto> Risks { get; set; }
        public IEnumerable<GetPoapDataProcedureDto> SubProcedures { get; set; }
    }
}
