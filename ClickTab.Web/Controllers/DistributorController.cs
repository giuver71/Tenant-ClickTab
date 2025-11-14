using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Registry;
using ClickTab.Core.DAL.Models.Tables;
using ClickTab.Core.EntityService.Generics;
using ClickTab.Core.EntityService.Registry;
using ClickTab.Web.Mappings;
using ClickTab.Web.Mappings.ModelsDTO.Registry;
using ClickTab.Web.Mappings.ModelsDTO.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClickTab.Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DistributorController : ControllerBase
    {
        private DistributorService _distributorService;
        private DatabaseContext _ctx;
        private AutoMappingService _autoMappingService;
        public DistributorController(DistributorService distributorService, DatabaseContext ctx, AutoMappingService autoMappingService)
        {
            _distributorService = distributorService;
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
            List<Distributor> models = _distributorService.GetAll();
            List<DistributorDTO> dtos = _autoMappingService.CurrentMapper.Map<List<DistributorDTO>>(models);
            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Distributor model = _distributorService.Get(id);
            DistributorDTO dto = _autoMappingService.CurrentMapper.Map<DistributorDTO>(model);

            return Ok(dto);
        }


        [HttpPost, Route("/api/[controller]")]
        public async Task<IActionResult> Save([FromBody] DistributorDTO dto)
        {
            Distributor model = _autoMappingService.CurrentMapper.Map<Distributor>(dto);
            int ID = _distributorService.Save(model);
            return Ok(ID);
        }

        [HttpDelete, Route("/api/[controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _distributorService.Delete(id);
            return Ok();
        }
    }
}