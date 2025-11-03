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
using ClickTab.Core.DAL.Models.Tables;
using ClickTab.Web.Mappings.ModelsDTO.Tables;
using Microsoft.AspNetCore.Authorization;
using ClickTab.DataTransfer;

namespace ClickTab.Web.Controllers.Generics
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataTransferController : ControllerBase
    {
        public DataTransferController(DatabaseContext ctx)
        {
        }

        /// <summary>
        /// Fuznione che restituisce la lista di tutti i Ruoli
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("/api/[controller]/Gruppi/{keyConnetcion}")]
        [AllowAnonymous]
        public async Task<IActionResult> Gruppi(string keyConnetcion)
        {
            var lista=ImportConfig.ToMappingGruppi(keyConnetcion);
            return Ok(lista);
        }


       

    }
}
