using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PoapInstrumentPacks.Dto
{
    [AutoMap(typeof(PoapInstrumentPack))]
    public class PoapInstrumentPackDto : EntityDto<int>
    {
        public Guid PoapId { get; set; }
        public Guid InstrumentPackId { get; set; }
        public int InstrumentId { get; set; }
        public string UserCode { get; set; }
        public double Quantity { get; set; }
        public double EmbodiedCarbon { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
