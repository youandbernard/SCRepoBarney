using Abp.Application.Services.Dto;
using System;

namespace CaseMix.Services.PatientSurveys.Dto
{
    public class PagedPatientSurveysRequestDto : PagedAndSortedResultRequestDto
    {

        public PagedPatientSurveysRequestDto()
        {
        }

        public string HospitalId { get; set; }
        public string Keyword { get; set; }
        public long? SurgeonId { get; set; }
        public string TheaterId { get; set; }
        public bool IsArchived { get; set; }
        public Guid? BodyStructureGroupId { get; set; }
        public string Timezone { get; set; }
    }
}
