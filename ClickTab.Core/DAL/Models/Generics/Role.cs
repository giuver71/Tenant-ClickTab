using EQP.EFRepository.Core.Attributes;
using EQP.EFRepository.Core.Interface;
using ClickTab.Core.HelperService.LookupEntityService.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Models.Generics
{
    [Serializable]
    [LookupClass(typeof(RoleLookupService), new string[] { "Description" }, IncludeFullObject = false)]
    public class Role : IBaseEntity, IAuditEntity<int> 
    {
        public int ID { get; set; }
        public string Description { get; set; }

        public int FK_InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

       public List<UserRole> UserRoles { get; set; }
       public List<RoleRule> RoleRules { get; set; } = new List<RoleRule>();

    }

  
}
