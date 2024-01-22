using Abp.Application.Services.Dto;

namespace CaseMix.Services.Hospitals.Dto
{
    public class PagedHospitalResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
