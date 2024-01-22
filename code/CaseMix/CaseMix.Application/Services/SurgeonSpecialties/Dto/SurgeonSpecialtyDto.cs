using Abp.AutoMapper;
using Abp.Domain.Entities;
using CaseMix.Entities;
using CaseMix.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.UserSpecialties.Dto
{
    [AutoMap(typeof(SurgeonSpecialty))]
    public class SurgeonSpecialtyDto
    {
        public long SurgeonId { get; set; }
        public Guid BodyStructureGroupId { get; set; }
        public string BodyStructureName { get; set; }
        public bool IsSelected { get; set; }
        public UserDto User { get; set; }
    }
}
