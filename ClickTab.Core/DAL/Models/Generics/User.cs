using ClickTab.Core.HelperService.LookupEntityService;
using EQP.EFRepository.Core.Attributes;
using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.DAL.Models.Generics
{
    [Serializable]
    [LookupClass(typeof(UserLookupService), new string[] { "Name", "Surname" }, IncludeFullObject = false)]
    public class User : IBaseEntity
    {
        public int ID { get; set; }
        // <ewz:fields area>
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public bool ChangedPassword { get; set; }

        public UserStatusEnum Status { get; set; }
        public DateTime SubscriptionDate { get; set; }

        public List<UserRole> UserRoles { get; set; }

        // <ewz:navigationArea>

        // <ewz:Interfaces area>

        // <ewz:enum area>

    }
    public enum UserStatusEnum
    {
        Abilitato = 1,
        Disabilitato = 2
    }

}

