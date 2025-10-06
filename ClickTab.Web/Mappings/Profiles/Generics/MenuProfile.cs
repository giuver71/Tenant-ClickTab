using AutoMapper;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Web.Mappings.ModelsDTO.Generics;

namespace ClickTab.Web.Mappings.Profiles.Generics
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuDTO>().ReverseMap();
        }
    }
}
