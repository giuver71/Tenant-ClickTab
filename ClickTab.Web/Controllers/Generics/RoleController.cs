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
    public class RoleController : ControllerBase
    {
        private RoleService _roleService;
        private DatabaseContext _dbContext;
        private AutoMappingService _autoMappingService;
        public RoleController(RoleService roleService, DatabaseContext dbContext, AutoMappingService autoMappingService)
        {
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
            List<Role> roles = _roleService.GetAll();
            List<RoleDTO> dtos = _autoMappingService.CurrentMapper.Map<List<RoleDTO>>(roles);
            return Ok(dtos);
        }


        

       

    }
}
