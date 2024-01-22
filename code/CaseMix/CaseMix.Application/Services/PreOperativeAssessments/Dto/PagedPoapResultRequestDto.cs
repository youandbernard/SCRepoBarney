using Abp.Application.Services.Dto;

namespace CaseMix.Services.PreOperativeAssessments.Dto
{
    public class PagedPoapResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string HospitalId { get; set; }
        public string Keyword { get; set; }
        public bool isDisplayRiskAwaitingCompletion { get; set; }
    }
}
