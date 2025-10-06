using ClickTab.Core.DAL.Models.Generics;
using System.Collections.Generic;
using System;

namespace ClickTab.Web.Mappings.ModelsDTO.Generics
{
    public class RoleDTO
    {
        public int ID { get; set; }
        public int? FK_Facilitie { get; set; }
        public string Description { get; set; }

        public List<MenuDTO> Menu { get; set; } = new List<MenuDTO>();
        public List<RoleRuleDTO> RoleRules { get; set; } = new List<RoleRuleDTO>();

        //public RoleDimensionEnum RoleDimension { get; set; }
        public int FK_InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
