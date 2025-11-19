using System;

namespace ClickTab.Web.Mappings.ModelsDTO.Registry
{
    public class DistributorDTO
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
