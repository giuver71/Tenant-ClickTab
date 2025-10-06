using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Models.Generics
{
    public class RoleRule : IBaseEntity
    {
        public int ID { get; set; }
        public int FK_Role { get; set; }
        public Role Role { get; set; }

        public int FK_Rule { get; set; }
        public Rule Rule { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        [NotMapped]
        public bool IsSelected {  get; set; }
        [NotMapped]
        public List<int> IdsDeleted { get; set; }
    }
}
