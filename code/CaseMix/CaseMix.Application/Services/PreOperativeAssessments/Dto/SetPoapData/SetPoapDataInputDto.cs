using System.Collections.Generic;

namespace CaseMix.Services.PreOperativeAssessments.Dto.SetPoapData
{
    public class SetPoapDataInputDto
    {
        public IEnumerable<SetPoapDataTimingDto> Timings { get; set; }
    }
}
