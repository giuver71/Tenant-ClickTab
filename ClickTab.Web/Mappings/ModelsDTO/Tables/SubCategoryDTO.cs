using System;

namespace ClickTab.Web.Mappings.ModelsDTO.Tables
{
    public class SubCategoryDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
