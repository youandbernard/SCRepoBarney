using System.Collections.Generic;

namespace CaseMix.Services.PreOperativeAssessments.Dto.SetPoapData
{
    public class SetPoapDataOutputDto
    {
        public SetPoapDataOutputDto()
        {
            PoapRecordsFailed = 0;
            Errors = new List<SetPoapDataErrorOutputDto>();
        }

        public int PoapRecordsReceived { get; set; }
        public int PoapRecordsFailed { get; set; }
        public List<SetPoapDataErrorOutputDto> Errors { get; set; }
    }
}
