using iText.Layout.Element;
using ClickTab.Core.DAL.Models.Generics;
using System.Collections.Generic;

namespace ClickTab.Web.Mappings.ModelsDTO.Generics
{
    public class RoleRuleDTO
    {
        public int ID { get; set; }
        public int FK_Rule { get; set; }
        public int FK_Role {  get; set; }
        public RuleDTO Rule { get; set; }
        public string RuleUrlRoutes { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool IsSelected { get; set; }
        public List<int> IdsDeleted { get; set; }
    }
}
