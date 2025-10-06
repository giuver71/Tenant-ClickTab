using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Models.Generics
{
    /// <summary>
    /// Classe che rappresenta il token di abilitazione al reset password da parte di un utente
    /// </summary>
    public class UrlToken : IBaseEntity, IAuditEntity<int>
    {
        public int ID { get; set; }

        public string Token { get; set; }
        public DateTime DtmExpiration { get; set; }

        public User User { get; set; }
        public int FK_User { get; set; }

        public int FK_InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public int FK_UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
