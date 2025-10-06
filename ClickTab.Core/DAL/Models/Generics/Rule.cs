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
    [LookupClass(typeof(RuleLookupService), new string[] { "Description" }, IncludeFullObject = false)]
    public class Rule : IBaseEntity
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string UrlRoutes { get; set; }
        public List<RoleRule> RoleRules { get; set; } = new List<RoleRule>();
    }
  
   
}
