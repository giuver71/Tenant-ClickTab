using Microsoft.AspNetCore.Mvc;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.EntityService.Generics;
using ClickTab.Web.Mappings;
using ClickTab.Web.Mappings.ModelsDTO.Generics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClickTab.Core.DAL.Enums;
using ClickTab.Core.HelperClass;

namespace ClickTab.Web.Controllers.Generics
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private MenuService _menuService;
        private RoleService _roleService;
        private DatabaseContext _dbContext;
        private AutoMappingService _autoMappingService;
        public MenuController(MenuService menuService, RoleService roleService, DatabaseContext dbContext, AutoMappingService autoMappingService)
        {
            _menuService = menuService;
            _roleService = roleService;
            _dbContext = dbContext;
            _autoMappingService = autoMappingService;
        }

        /// <summary>
        /// Fuznione che restituisce la lista di tutti i Ruoli
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("/api/[controller]/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Menu> Menus = _menuService.GetAll();
            Menus = Menus.OrderBy(x => x.Label).ToList();
            List<MenuDTO> MenuDTOs = _autoMappingService.CurrentMapper.Map<List<MenuDTO>>(Menus);
            return Ok(MenuDTOs);
        }

        /// <summary>
        /// Recupera l'elenco piatto delle voci di menù, considerando solo le voci parent che non hanno figli o solo le voci figlie.
        /// L'elenco recuperato è filtrato in base ai ruoli di sistema e dimensione.
        /// Attualmente l'API è utilizzata per recuperare l'elenco delle voci di menù per ruolo
        /// </summary>
        /// <param name="systemRole"></param>
        /// <param name="roleDimension"></param>
        /// <returns></returns>
        //[HttpGet, Route("/api/[controller]/GetFlattedMenuBySystemRole/{systemRole}")]///{roleDimension?}
        //public async Task<IActionResult> GetFlattedMenuBySystemRole(SystemRole systemRole)//RoleDimensionEnum? roleDimension
        //{
        //    List<Menu> menu = _menuService.GetBy(m => m.SystemRole == null || m.SystemRole.Value.HasFlag(systemRole), p => p.Children);

        //    //if (roleDimension.HasValue)
        //    //    menu = menu.Where(m => m.RoleDimension == null || m.RoleDimension.Value.HasFlag(roleDimension.Value)).ToList();
        //    //Filtra eliminando le voci parent che hanno figli
        //    menu = menu.Where(p => (p.FK_Parent == null && !p.Children.Any()) || p.FK_Parent != null).ToList();

        //    List<MenuDTO> menuDTO = _autoMappingService.CurrentMapper.Map<List<MenuDTO>>(menu);
        //    return Ok(menuDTO);
        //}

        [HttpGet, Route("/api/[controller]/GetMenuNavItemsByRole/{roleId?}")]
        public async Task<IActionResult> GetMenuNavItemsByRole(SystemRole systemRole, int? roleId = null)
        {
            List<Menu> menu = _menuService.GetMenuHierarchy(roleId);
            //List<MenuDTO> menuDTO = _autoMappingService.CurrentMapper.Map<List<MenuDTO>>(menu);
            List<NavItem> navitems = _autoMappingService.CurrentMapper.Map<List<NavItem>>(menu);

            return Ok(navitems);
        }

        [HttpGet, Route("/api/[controller]/GetMenuByRole/{roleId?}")]
        public async Task<IActionResult> GetMenuByRole(SystemRole systemRole, int? roleId = null)
        {
            List<Menu> menu = _menuService.GetMenuHierarchy(roleId);
            List<MenuDTO> menuDTO = _autoMappingService.CurrentMapper.Map<List<MenuDTO>>(menu);
            return Ok(menuDTO);
        }

        /// <summary>
        /// Restiusce il Ruolo in cui ID è dato come paramewtro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Menu menu = _menuService.Get(id);
            MenuDTO menuDTO = _autoMappingService.CurrentMapper.Map<MenuDTO>(menu);
            return Ok(menuDTO);
        }

    }
}
