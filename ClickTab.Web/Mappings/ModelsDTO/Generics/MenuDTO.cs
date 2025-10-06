using System.Collections.Generic;

namespace ClickTab.Web.Mappings.ModelsDTO.Generics
{
    public class MenuDTO
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public string ClientCode { get; set; } // Campo TranslateKey
        public string ParentClientCode { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool normallyHaveChildren { get; set; }
        public bool isExternalPage { get; set; }
        public List<MenuDTO> children { get; set; }
    }
}
