using AutoMapper;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Web.Mappings.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.Profiles.Generics
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
