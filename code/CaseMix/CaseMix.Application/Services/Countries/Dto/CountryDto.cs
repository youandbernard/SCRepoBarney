using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Countries.Dto
{
    [AutoMap(typeof(Country))]
    public class CountryDto : EntityDto<int>
    {
        public string Name { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
