using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;

namespace CaseMix.Services.Theaters.Dto
{
    [AutoMap(typeof(Theater))]
    public class TheaterDto : EntityDto<Guid>
    {
        public string TheaterId { get; set; }
        public string Name { get; set; }
        public string HospitalId { get; set; }
    }
}
