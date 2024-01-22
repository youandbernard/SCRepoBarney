using Abp.Application.Services.Dto;

namespace CaseMix.Services.Theaters.Dto
{
    public class PagedTheaterResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string HospitalId { get; set; }
        public string Keyword { get; set; }
    }
}
