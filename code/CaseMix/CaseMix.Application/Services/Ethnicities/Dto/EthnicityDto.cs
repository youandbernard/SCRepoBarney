using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;

namespace CaseMix.Services.Ethnicities.Dto
{
    [AutoMap(typeof(Ethnicity))]
    public class EthnicityDto : EntityDto<int>
    {
        public string Description { get; set; }
    }
}
