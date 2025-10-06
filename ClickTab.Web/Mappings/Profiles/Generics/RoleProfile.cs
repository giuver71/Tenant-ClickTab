using AutoMapper;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Web.Mappings.ModelsDTO.Generics;

namespace ClickTab.Web.Mappings.Profiles.Generics
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDTO>();


            CreateMap<RoleDTO, Role>();
              

        }
    }
}
