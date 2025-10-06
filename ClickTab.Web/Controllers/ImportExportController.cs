using ClickTab.Core.HelperService;
using EQP.EFRepository.Core.Services;
using ClickTab.Web.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExportController : ControllerBase
    {
        private EQPImportExportService _importExportService;

        public ImportExportController(EQPImportExportService csvService)
        {
            _importExportService = csvService;
        }

        /// <summary>
        /// Restituisce il tracciato record che descrive i dati dell'entità
        /// avente come Type quello indicato nel parametro EntityTypeName
        /// </summary>
        /// <remarks>
        /// Se per l'entità non è stato definito il decoratore per l'import/export tramite CSV allora
        /// restituisce un'eccezione e interrompe la procedura
        /// </remarks>
        /// <param name="EntityTypeName">Nome del Type della classe per cui richiedere il tracciato record d'esempio</param>
        /// <returns>Restituisce un file csv</returns>
        [HttpGet, Route("/api/[controller]/GetCSVTrack/{EntityTypeName}")]
        public async Task<IActionResult> GetCSVTrack(string EntityTypeName)
        {
            ImportExportConfig config = new ImportExportConfig()
            {
                EntityTypeName = EntityTypeName,
                ApplyFormatToLabels = true
            };
            byte[] exportedCsv = _importExportService.ExportXlsxTemplate(config);
            return new FileContentResult(exportedCsv, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// A partire dal type della classe e dal file csv passato nel corpo della request esegue l'importazione
        /// dei dati presenti nel csv all'interno della tabella mappata dall'entità avente il type richiesto
        /// </summary>
        /// <param name="EntityTypeName">Nome del type della classe per cui effettuare l'import da csv</param>
        /// <returns></returns>
        [HttpPost, Route("/api/[controller]/ImportCsv/{EntityTypeName}")]
        public async Task<IActionResult> ImportCsv([FromRoute] string EntityTypeName)
        {
            if (Request.Form.Files == null || !Request.Form.Files.Any())
                throw new Exception("Must declare the csv file for import data");

            var csvToImport = Request.Form.Files[0];
            byte[] csvFile = null;
            using (var ms = new MemoryStream())
            {
                csvToImport.CopyTo(ms);
                csvFile = ms.ToArray();
            }

            Dictionary<string, object> externalDictionary = new Dictionary<string, object>();
            if (Request.Form.Keys.Contains("additionalParams"))
            {
                externalDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Request.Form["additionalParams"][0]);
            }

            ImportExportConfig config = new ImportExportConfig()
            {
                EntityTypeName = EntityTypeName,
                ExternalPropertyValues = externalDictionary
            };
            byte[] resultFile = _importExportService.ImportXlsx(config, csvFile, out bool hasErrors);

            CsvImportResultDTO result = new CsvImportResultDTO();
            result.HasErrors = hasErrors;

            if (hasErrors == true)
            {
                result.FileContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                result.FileBase64 = Convert.ToBase64String(resultFile);
            }

            return Ok(result);
        }
    }
}
