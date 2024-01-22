using System;

namespace CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData
{
    public class GetPoapDataProcedureDto
    {
        public int OrderId { get; set; }
        public Guid ProcedureId { get; set; }
        public string SnomedId { get; set; }
        public string Name { get; set;}
        public string ProcedureSite { get; set; }
        public string Method { get; set; }
        public double? MeanTime { get; set; }
        public double? StandardDeviation { get; set; }
        public bool IsRisk { get; set; }
    }
}
