using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Repository.Generics;
using ClickTab.Core.HelperService;
using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.EntityService.Generics
{
    public class UrlTokenService : IdentityService<UrlTokenRepository, UrlToken>
    {
        private PasswordService _passwordService;
        public UrlTokenService(UnitOfWork<DatabaseContext> uow, PasswordService passwordService) : base(uow)
        {
            _passwordService = passwordService;
        }

        /// <summary>
        /// Metodo che genera un token Guid, lo critta con SHA256 associandolo ad uno User mettendo data scadenza a mezz'ora da adesso, 
        /// inoltre elimina eventuali token creati precedentemente per lo stesso user
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GenerateToken(User currentUser)
        {
            Guid newGuid = Guid.NewGuid();
            string token = _passwordService.EncryptSHA256(newGuid.ToString());

            UrlToken urlToken = new UrlToken()
            {
                DtmExpiration = DateTime.Now.AddMinutes(30),
                FK_User = currentUser.ID,
                Token = token
            };

            //Disattivo eventuali token che sono stati creati precedentemente (Es. l'utente chiede 2 volte il cambio password perchè non è arrivata l'email o per errore)
            List<UrlToken> urlTokens = GetBy(a => a.FK_User == currentUser.ID);

            try
            {
                _uow.BeginTransaction();
                if (urlTokens.Any())
                {
                    base.Delete(urlTokens);
                }

                base.Save(urlToken);
                _uow.CommitTransaction();
            }
            catch (Exception ex)
            {
                _uow.RollbackTransaction();
                throw new Exception("Si è verificato un problema con il cambio password, riprovare più tardi");
            }

            return token;
        }
    }
}
