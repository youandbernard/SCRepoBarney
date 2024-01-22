namespace CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData
{
    public class GetPoapDataRiskDto
    {
        public string RiskGroup { get; set; }
        public string RiskKey { get; set; }
        public string RiskValue { get; set; }
        public double? MeanTime { get; set; }
        public double? StandardDeviation { get; set; }
    }
}
