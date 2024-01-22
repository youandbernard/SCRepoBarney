using System;
using System.Collections.Generic;

namespace CaseMix.Services.PreOperativeAssessments.Dto.SetPoapData
{
    public class SetPoapDataTimingDto
    {
        public Guid PoapId { get; set; }
        public IEnumerable<SetPoapDataSubProcedureDto> SubProcedures { get; set; }
    }
}
