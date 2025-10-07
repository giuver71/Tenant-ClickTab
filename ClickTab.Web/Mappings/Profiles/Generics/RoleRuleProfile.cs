using AutoMapper;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Web.Mappings.ModelsDTO.Generics;

namespace ClickTab.Web.Mappings.Profiles.Generics
{
    public class RoleRuleProfile : Profile
    {
        public RoleRuleProfile()
        {
            CreateMap<RoleRule, RoleRuleDTO>();
            CreateMap<RoleRuleDTO, RoleRule>()
                .ForMember(dest => dest.Rule, opt => opt.Ignore());
        }
    }
}
