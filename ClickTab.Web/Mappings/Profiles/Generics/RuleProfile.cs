using AutoMapper;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Web.Mappings.ModelsDTO.Generics;

namespace ClickTab.Web.Mappings.Profiles.Generics
{
    public class RuleProfile : Profile
    {
        public RuleProfile()
        {
            CreateMap<Rule, RuleDTO>().ReverseMap();
        }
    }
}
