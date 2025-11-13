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
    public class SubCategoryController : ControllerBase
    {
        private SubCategoryService _subCategoryService;
        private DatabaseContext _ctx;
        private AutoMappingService _autoMappingService;
        public SubCategoryController(SubCategoryService SubCategoryService, DatabaseContext ctx, AutoMappingService autoMappingService)
        {
            _subCategoryService = SubCategoryService;
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
            List<SubCategory> subcategories = _subCategoryService.GetAll();
            List<SubCategoryDTO> dtos = _autoMappingService.CurrentMapper.Map<List<SubCategoryDTO>>(subcategories);
            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            SubCategory model = _subCategoryService.Get(id);
            SubCategoryDTO dto = _autoMappingService.CurrentMapper.Map<SubCategoryDTO>(model);

            return Ok(dto);
        }


        [HttpPost, Route("/api/[controller]")]
        public async Task<IActionResult> Save([FromBody] SubCategoryDTO dto)
        {
            SubCategory model = _autoMappingService.CurrentMapper.Map<SubCategory>(dto);
            int ID = _subCategoryService.Save(model);
            return Ok(ID);
        }

        [HttpDelete, Route("/api/[controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _subCategoryService.Delete(id);
            return Ok();
        }

    }
}
