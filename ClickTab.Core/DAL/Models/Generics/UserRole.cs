using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Models.Generics
{
    public class UserRole : IBaseEntity, IAuditEntity<int>
    {
        public int ID { get; set; }

        public int FK_User { get; set; }
        public User User { get; set; }

        public int FK_Role { get; set; }
        public Role Role { get; set; }

        public int FK_InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        
    }
}
