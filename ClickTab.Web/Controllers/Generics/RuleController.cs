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
using ClickTab.Web.Mappings.ModelsDTO;

namespace ClickTab.Web.Controllers.Generics
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private RuleService _ruleService;
        private DatabaseContext _dbContext;
        private AutoMappingService _autoMappingService;
        public RuleController(RuleService ruleService, DatabaseContext dbContext, AutoMappingService autoMappingService)
        {
            _ruleService = ruleService;
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
            List<Rule> rules = _ruleService.GetAll();
            List<RuleDTO> dtos = _autoMappingService.CurrentMapper.Map<List<RuleDTO>>(rules);
            return Ok(dtos);
        }

       






    }
}
