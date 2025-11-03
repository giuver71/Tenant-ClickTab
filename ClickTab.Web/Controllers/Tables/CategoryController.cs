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
    public class CategoryController : ControllerBase
    {
        private CategoryService _categoryService;
        private DatabaseContext _ctx;
        private AutoMappingService _autoMappingService;
        public CategoryController(CategoryService CategoryService, DatabaseContext ctx, AutoMappingService autoMappingService)
        {
            _categoryService = CategoryService;
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
            List<Category> categories = _categoryService.GetAll();
            List<CategoryDTO> dtos = _autoMappingService.CurrentMapper.Map<List<CategoryDTO>>(categories);
            return Ok(dtos);
        }


        [HttpGet, Route("/api/[controller]/GetCityById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category model = _categoryService.Get(id);
            CategoryDTO dto = _autoMappingService.CurrentMapper.Map<CategoryDTO>(model);

            return Ok(dto);
        }


        [HttpPost, Route("/api/[controller]")]
        public async Task<IActionResult> Save([FromBody] CategoryDTO dto)
        {
            Category model = _autoMappingService.CurrentMapper.Map<Category>(dto);
            int ID = _categoryService.Save(model);
            return Ok(ID);
        }

        [HttpDelete, Route("/api/[controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _categoryService.Delete(id);
            return Ok();
        }

    }
}
