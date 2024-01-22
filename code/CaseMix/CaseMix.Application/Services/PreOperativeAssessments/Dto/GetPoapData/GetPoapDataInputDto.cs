using System;

namespace CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData
{
    public class GetPoapDataInputDto
    {
        public long SurgeonId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
