using System.Collections.Generic;

namespace CaseMix.Services.PreOperativeAssessments.Dto.GetPoapData
{
    public class GetPoapDataOutputDto
    {
        public long SurgeonId { get; set; }
        public string SurgeonName { get; set; }

        public IEnumerable<GetPoapDataPoapDto> Poap { get; set; }
    }
}
