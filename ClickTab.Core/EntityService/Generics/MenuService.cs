using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Services;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Repository.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickTab.Core.DAL.Enums;

namespace ClickTab.Core.EntityService.Generics
{
    public class MenuService : IdentityService<MenuRepository, Menu>
    {
        private RoleService _roleService;
        public MenuService(UnitOfWork<DatabaseContext> uow, RoleService roleService) : base(uow)
        {
            _roleService = roleService;
        }

        public List<Menu> GetFlattedMenuByRole(SystemRole SystemRole, int? FK_Role = null)
        {
            List<Menu> allMenuItems = new List<Menu>();

            //STEP 1: Recupera tutte le voci di menù che hanno SystemRole = NULL oppure uguale al valore del parametro passato.
            allMenuItems = GetBy(m => m.SystemRole == null || m.SystemRole.Value.HasFlag(SystemRole), p => p.Children);

            //STEP 2: Se ruolo diverso da ADMIN allora filtra prendendo solo le voci
            if (SystemRole != SystemRole.ADMIN)
            {
                //Role fullRole = FK_Role.HasValue ? _roleService.Get(FK_Role.Value, r => r.MenuRole) : new Role() { MenuRole = new List<MenuRole>() };
                //allMenuItems = allMenuItems.Where(c => c.RoleDimension == null || c.RoleDimension.Value.HasFlag(fullRole.RoleDimension)).ToList();

                Role fullRole = FK_Role.HasValue ? _roleService.Get(FK_Role.Value) : new Role();
            }

            return allMenuItems;
        }

        /// <summary>
        /// Restituisce l'elenco gerarchico delle voci di menu, opportunamente filtrate
        /// in base al ruolo di sistema (tra SUPERADMIN e USER) e ruolo utente (valido solo se SystemRole = USER).
        /// In caso di SystemRole = USER, la funzione si occupa anche di filtrare le voci di menù in base al ruolo richiesto tramite
        /// il parametro FK_Role, in questo modo sarà possibile differenziare le voci di menù in base Alle Regole associate al ruolo definbito
        /// </summary>
        /// <param name="SystemRole">Valore del ruolo di sistema per cui recuperare le voci di menu</param>
        /// <param name="FK_Role">ID del ruolo utente (opzionale)</param>
        /// <returns></returns>
        public List<Menu> GetMenuHierarchy(SystemRole SystemRole, int? FK_Role = null)
        {
            List<Menu> allMenuItems = new List<Menu>();

            //Se SystemRole è diverso da ADMIN ed è stato passato il ruolo utente allora recupera il ruolo completo per avere le entità MenuRole
            //Le entità MenuRole definiscono le voci di menù gestibili per ciascun ruolo applicativo quindi in questo step verrà
            //filtrato ulteriormente l'elenco dei menù escludendo le voci non accessibili per il ruolo richiesto

            //Role fullRole = FK_Role.HasValue ? _roleService.Get(FK_Role.Value, r => r.MenuRole) : new Role() { MenuRole = new List<MenuRole>() };

            // Recuopera tutti gli url relativi alla regola abbionata al ruolo e li splitta in un array di stringhe
            Role fullRole = FK_Role.HasValue ? _roleService.GetFullRole(FK_Role.Value) : new Role() { RoleRules = new List<RoleRule>() };
            if (!fullRole.RoleRules.Any())
                return allMenuItems;

            // Array di strunghe contenete tutti gli Url Consentiti dalla regole associate al Ruolo
            string[] canUrls = fullRole.RoleRules.Any() ? fullRole.RoleRules.Where(p => p.Rule.UrlRoutes != null).Select(x => x.Rule.UrlRoutes).SelectMany(p => p.Split(';')).ToArray() : null;


            //allMenuItems = GetBy(m => m.FK_Parent == null && (m.SystemRole == null || m.SystemRole.Value.HasFlag(SystemRole)
            //                                              && (m.RoleDimension == null || m.RoleDimension.Value.HasFlag(fullRole.RoleDimension))), p => p.Children);

            //STEP 1: Recupera tutte le voci di menù PARENT che hanno SystemRole = NULL oppure uguale al valore del parametro passato.
            //         E quelli contenuti nel campo delle regole associate oppure quelli con SisytemRole ADMIN
            //        Per ciascuna voce di menù parent recuperata include anche le eventuali voci figlie (cioè i Children)

            allMenuItems = GetBy(m => m.FK_Parent == null && (m.SystemRole == null || m.SystemRole.Value.HasFlag(SystemRole) && (canUrls.Contains(m.Url) || SystemRole == SystemRole.ADMIN)), p => p.Children);


            Action<Menu> CheckMenuRole = null;
            CheckMenuRole = menuItem =>
            {
                //Filtra le voci di menù figlie in base al SystemRole
                menuItem.Children = menuItem.Children.Where(c => c.SystemRole == null || c.SystemRole.Value.HasFlag(SystemRole)).ToList();

                //Se SystemRole è diverso da ADMIN allora filtra ulteriormente le voci di menù figlie per
                //eliminare eventuali voci che non risultano associate alla dimensione del ruolo avente ID uguale al parametro FK_Role
                if (SystemRole != SystemRole.ADMIN)
                {
                    //Elimina eventuali voci di menù figlie che non risultano associate alla RoleDimension del ruolo applicativo avente ID uguale
                    //al parametro FK_Role passato
                    //menuItem.Children = menuItem.Children.Where(c => c.RoleDimension == null || c.RoleDimension.Value.HasFlag(fullRole.RoleDimension)).ToList();

                    //Elimina eventuali voci di menu che non risultano associate al ruolo all'interno della tabella MenuRole
                    //menuItem.Children = fullRole.RoleDimension == DAL.Enums.RoleDimensionEnum.ADMIN ? menuItem.Children : menuItem.Children.Where(c => fullRole.MenuRole.Any(m => m.FK_Menu == c.ID)).OrderBy(c => c.Order).ToList();

                    // ******** Workaround regole 
                    // Elimina eventuali voci di menu che il cui url non risulta associato alla regola abbinata al ruolo
                    menuItem.Children = menuItem.Children.Where(p => canUrls.Contains(p.Url)).OrderBy(c => c.Order).ToList();
                }

                //Ordina l'elenco dei figli in base alla proprietà Order
                menuItem.Children = menuItem.Children.OrderBy(c => c.Order).ToList();

                //Se c'è un ulteriore livello di gerarchia allora scende ad ispezionare anche quella
                foreach (Menu child in menuItem.Children)
                {
                    if (child.Children != null && child.Children.Any())
                        CheckMenuRole(child);
                }
            };

            //Avvia la funzione ricorsiva per il controllo delle voci di menu per ciascun elemento risultante dallo STEP 1
            List<Menu> parentItemToRemove = new List<Menu>();
            foreach (Menu menu in allMenuItems)
            {
                CheckMenuRole(menu);
                //Se la voce di menu parent risulta non avere più figli dopo la normalizzazione con ruoli e permessi allora ne tiene traccia per l'eliminazione
                if ((menu.Children == null || !menu.Children.Any()) && Exists(c => c.FK_Parent == menu.ID))
                    parentItemToRemove.Add(menu);
            }

            foreach (Menu menuToRemove in parentItemToRemove)
                allMenuItems.Remove(menuToRemove);

            return allMenuItems.OrderBy(p => p.Order).ToList();
        }
    }
}
