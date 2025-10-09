using System;

namespace ClickTab.Web.Mappings.ModelsDTO.Generics
{
    public class UserRoleDTO
    {
        public int ID { get; set; }

        public int FK_User { get; set; }

        public int FK_Role { get; set; }

        public RoleDTO Role { get; set; }
        
        public int FK_InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Deleted { get; set; }
        public int? FK_DeletedUser { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
