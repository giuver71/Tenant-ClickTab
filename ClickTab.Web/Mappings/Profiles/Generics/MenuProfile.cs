using AutoMapper;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.HelperClass;
using ClickTab.Web.Mappings.ModelsDTO.Generics;

namespace ClickTab.Web.Mappings.Profiles.Generics
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuDTO>().ReverseMap();
            CreateMap<Menu, NavItem>()
                 .ForMember(dto => dto.url, opt => opt.MapFrom(model => model.Url))
                 .ForMember(dto => dto.icon, opt => opt.MapFrom(model => model.Icon))
                 .ForMember(dto => dto.name, opt => opt.MapFrom(model => model.ClientCode));

        }
    }
}
