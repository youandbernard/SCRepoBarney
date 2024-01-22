using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PoapInstrumentPacks.Dto
{
    [AutoMap(typeof(InstrumentPack))]
    public class InstrumentPackDto : EntityDto<int>
    {
        public Guid InstrumentPackId { get; set; }
        public string PackName { get; set; }
        public int PackType { get; set; }
        public double? EmbodiedCarbon { get; set; }
        public string Specialty { get; set; }
        public string Company { get; set; }
    }
}
