using EQP.EFRepository.Core.Helpers;
using EQP.EFRepository.Core.Interface;
using EQP.EFRepository.Core.Models;
using EQP.EFRepository.Core.Services;
using ClickTab.Web.Mappings;
using ClickTab.Web.Mappings.ModelsDTO;
using ClickTab.Web.Mappings.ModelsDTO.Generics;
using HSE.Core.HelperService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private EQPLookupServiceLocator _lookupService;
        private EQPLookupServiceStringLocator _lookupStringService;
        private AutoMappingService _mappingService;

        public LookupController(AutoMappingService mappingService, EQPLookupServiceLocator lookupService, EQPLookupServiceStringLocator lookupStringService)
        {
            _mappingService = mappingService;
            _lookupService = lookupService;
            _lookupStringService = lookupStringService;
        }

        /// <summary>
        /// Restituisce l'elenco di elementi di tipo LookupDTO da mostrare in una specifica lookup del client.
        /// L'elenco di elementi restituito varia in base al tipo di oggetto richiesto (Type del modello di EF).
        /// </summary>
        /// <example>
        /// Recupera tutti gli oggetti di tipo User per la lookup
        /// <c>api/lookup/GetLookupEntities/User</c>
        /// </example>
        /// <param name="typeName">Type della classe per cui recuperare gli oggetti per la lookup</param>
        /// <returns>Restituisce una lista di oggetti di tipo LookupDTO</returns>
        [HttpPost, Route("/api/lookup/GetLookupEntities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLookupEntities([FromBody] LookupConfigDTO config)
        {
            List<LookupModel> entities = null;

            //Ricostruisce i filtri, se presenti
            List<ComplexLinqPredicate> complexLinqPredicates = config.ComplexFilters != null ? _mappingService.CurrentMapper.Map<List<ComplexLinqPredicate>>(config.ComplexFilters) : null;
            List<LinqPredicate> simpleLinqPredicates = config.Filters != null ? _mappingService.CurrentMapper.Map<List<LinqPredicate>>(config.Filters) : null;

            //Recupera gli elementi di lookup e li mappa in oggetti LookupDTO
            Type entityType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).FirstOrDefault(t => !t.IsSubclassOf(typeof(Migration)) && t.Name.ToLower() == config.TypeName.ToLower());
            if(typeof(IBaseEntity).IsAssignableFrom(entityType))
                entities = _lookupService.GetLookupEntities(config.TypeName, config.CustomConfig, simpleLinqPredicates, complexLinqPredicates);
            else if (typeof(IBaseStringEntity).IsAssignableFrom(entityType))
                entities = _lookupStringService.GetLookupEntities(config.TypeName, config.CustomConfig, simpleLinqPredicates, complexLinqPredicates);

            List<LookupDTO> lookupSource = _mappingService.CurrentMapper.Map<List<LookupDTO>>(entities);
            return Ok(lookupSource);
        }
    }
}
