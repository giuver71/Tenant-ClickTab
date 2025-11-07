using System;

namespace ClickTab.Web.Mappings.ModelsDTO.Tables
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal Fee { get; set; }
        public bool IsFiscal { get; set; }
        public string Department { get; set; }
        public bool Negative { get; set; }
        public bool FeeOnPurchasePrice { get; set; }
        public int FK_InsertUser { get; set; }

        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
