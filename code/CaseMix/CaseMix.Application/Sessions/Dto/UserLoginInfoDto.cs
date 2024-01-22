using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CaseMix.Authorization.Users;
using CaseMix.Services.Manufactures.Dto;
using System;
using System.Collections.Generic;

namespace CaseMix.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public bool DisplayCompletedSurvey { get; set; }
        public bool IsAdmin { get; set; }
        public IList<string> RoleNames { get; set; }

        public Guid? ManufactureId { get; set; }
    }
}
