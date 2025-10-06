using EQP.EFRepository.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO
{
    public class ComplexLinqPredicateDTO
    {
        public List<LinqPredicateDTO> Predicates { get; set; } = new List<LinqPredicateDTO>();
    }

    public class LinqPredicateDTO
    {
        public List<LinqFilterDTO> PropertyFilters { get; set; } = new List<LinqFilterDTO>();
    }

    public class LinqFilterDTO
    {
        /// <summary>
        /// Nome della proprietà (dell'entità principale) su cui applicare il filtro.
        /// Il nome può essere composto da più livelli di navigazione a patto che ogni livello sia separato dal punto (es User.ID)
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Valore di confronto per la where part.
        /// In caso di RelationType = ContainsElement oppure RelationType = NotContainsElement se questo campo viene lasciato NULL
        /// allora verrà verificata la condizione per cui la lista annidata di elementi contiene almeno un elemento oppure no (cioè verrà applicato soltanto l'operatore Any() oppure !Any())
        /// </summary>
        public object PropertyValue { get; set; }

        /// <summary>
        /// Identifica il tipo di operando da usare nella where part
        /// </summary>
        public LinqFilterType RelationType { get; set; }

        /// <summary>
        /// Da definire solo per RelationType uguale a ContainsElement o NotContainsElement.
        /// Serve per indicare la proprietà degli elementi della lista annidata su cui applicare il filtro richiesto.
        /// </summary>
        public string ListElementPropertyName { get; set; }
    }
}
