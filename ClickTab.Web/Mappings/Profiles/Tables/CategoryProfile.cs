using AutoMapper;
using ClickTab.Core.DAL.Models.Tables;
using ClickTab.Web.Mappings.ModelsDTO.Generics;
using ClickTab.Web.Mappings.ModelsDTO.Tables;

namespace ClickTab.Web.Mappings.Profiles.Tables
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
