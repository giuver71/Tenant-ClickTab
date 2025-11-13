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
using System;

namespace ClickTab.Web.Controllers.Generics
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataTransferController : ControllerBase
    {
        private CategoryService _categoryService;
        private SubCategoryService _subCategoryService;
        public DataTransferController(DatabaseContext ctx, CategoryService categoryService, SubCategoryService subCategoryService)
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
        }

        /// <summary>
        /// Fuznione che restituisce la lista di tutti i Ruoli
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("/api/[controller]/Gruppi/{keyConnetcion}")]
        [AllowAnonymous]
        public async Task<IActionResult> Gruppi(string keyConnetcion)
        {
            var lista = ImportConfig.ToMappingGruppi(keyConnetcion);
            _categoryService.Save(lista);
            return Ok(1);
        }



        /// <summary>
        /// Fuznione che restituisce la lista di tutti i Ruoli
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("/api/[controller]/SottoGruppi/{keyConnetcion}")]
        [AllowAnonymous]
        public async Task<IActionResult> SottoGruppi(string keyConnetcion)
        {
            var lista = ImportConfig.ToMappingSottoGruppi(keyConnetcion);

            foreach (var item in lista)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    item.Code = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(item.Description))
                {
                    item.Description = Guid.NewGuid().ToString();
                }
            }
            _subCategoryService.Save(lista);
            return Ok(1);
        }

    }
}
