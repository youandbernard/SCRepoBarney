using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.Manufactures.Dto
{
    [AutoMap(typeof(Manufacture))]
    public class ManufactureDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
