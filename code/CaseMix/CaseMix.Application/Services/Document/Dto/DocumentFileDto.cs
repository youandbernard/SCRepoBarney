using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaseMix.Services.Document.Dto
{
    [AutoMap(typeof(DocumentFile))]
    public class DocumentFileDto: EntityDto<int>
    {
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public string Filetype { get; set; }
        public long Filelength { get; set; }
        public int DocumentType { get; set; }
        public bool Enable { get; set; }
        public bool Active { get; set; }
        public long UserId { get; set; }
        public DateTime DateUploaded { get; set; }
        public Guid? ManufacturerId { get; set; }

        [NotMapped]
        public string Manufacturer { get; set; }
        [NotMapped]
        public string SpecialtyName { get; set; }
        [NotMapped]
        public string DeviceClassName { get; set; }
        [NotMapped]
        public string DeviceFamilyName { get; set; }
    }
}
