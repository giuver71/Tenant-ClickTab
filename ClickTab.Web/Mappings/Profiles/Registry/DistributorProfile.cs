using AutoMapper;
using ClickTab.Core.DAL.Models.Registry;
using ClickTab.Core.DAL.Models.Tables;
using ClickTab.Web.Mappings.ModelsDTO.Registry;
using ClickTab.Web.Mappings.ModelsDTO.Tables;

namespace ClickTab.Web.Mappings.Profiles.Registry
{
    public class DistributorProfile : Profile
    {
        public DistributorProfile()
        {
            CreateMap<Distributor, DistributorDTO>().ReverseMap();
        }
    }
}
