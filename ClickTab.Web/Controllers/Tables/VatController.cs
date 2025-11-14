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

namespace ClickTab.Web.Controllers.Generics
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatController : ControllerBase
    {
        private VatService _vatService;
        private DatabaseContext _ctx;
        private AutoMappingService _autoMappingService;
        public VatController(VatService vatService, DatabaseContext ctx, AutoMappingService autoMappingService)
        {
            _vatService = vatService;
            _ctx = ctx;
            _autoMappingService = autoMappingService;
        }

        /// <summary>
        /// Fuznione che restituisce la lista di tutti i Ruoli
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("/api/[controller]/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Vat> models = _vatService.GetAll();
            List<VatDTO> dtos = _autoMappingService.CurrentMapper.Map<List<VatDTO>>(models);
            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Vat model = _vatService.Get(id);
            VatDTO dto = _autoMappingService.CurrentMapper.Map<VatDTO>(model);

            return Ok(dto);
        }


        [HttpPost, Route("/api/[controller]")]
        public async Task<IActionResult> Save([FromBody] VatDTO dto)
        {
            Vat model = _autoMappingService.CurrentMapper.Map<Vat>(dto);
            int ID = _vatService.Save(model);
            return Ok(ID);
        }

        [HttpDelete, Route("/api/[controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _vatService.Delete(id);
            return Ok();
        }

    }
}
