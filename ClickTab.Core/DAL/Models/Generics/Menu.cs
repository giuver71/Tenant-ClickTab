using EQP.EFRepository.Core.Attributes;
using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ClickTab.Core.DAL.Enums;

namespace ClickTab.Core.DAL.Models.Generics
{
    [Serializable]
    public class Menu : IBaseEntity, IAuditEntity<int>, ISoftDeleteEntity
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public string ClientCode { get; set; } // Campo TranslateKey
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public SystemRole? SystemRole { get; set; }


        public ExternalActionEnum isExternalPage { get; set; } // true = Vuol dire che la pagina non fa parte del menu, ma è una pagina interna sulla quale bisogna fare il controllo sugli accessi
        public bool normallyHaveChildren { get; set; }

        [JsonIgnore]
        public Menu Parent { get; set; }
        public int? FK_Parent { get; set; }
        public List<Menu> Children { get; set; } = new List<Menu>();
        public int FK_InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Deleted { get; set; }
        public int? FK_DeletedUser { get; set; }
        public DateTime? DeletedDate { get; set; }

    }

    public enum ExternalActionEnum
    {
        ROUTE = 0,
        NEWPAGE = 1
    }
}
