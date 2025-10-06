using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.HelperService;
using ClickTab.Web.NotificationCenter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClickTab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private FileService _fileService;
        private EmailService _emailService;
        private DatabaseContext _ctx;
        public TestController(FileService fileService, EmailService emailService,DatabaseContext ctx)
        {
            _fileService = fileService;
            _emailService = emailService;
            _ctx = ctx;
        }

        // GET: api/<TestController>
        [HttpGet, Route("/api/[controller]/TestList")]
        [AllowAnonymous]
        public async Task<IActionResult> TestList()
        {
            return Ok(_ctx.Users.ToList());
        }

        [HttpGet, Route("/api/[controller]/TestEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> TestEmail()
        {
            User newUser = new User()
            {
                Email = "ale.disalvatore@gmail.com",
                Name = "Alessandro",
                Surname = "Di Salvatore"
            };
            _emailService.ResetPassword(newUser, "123456");
            return Ok();
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            return Ok("value 1");
        }

        // POST api/<TestController>
        [HttpPost]
        [AllowAnonymous]
        public void Post([FromBody] dynamic value)
        {
            //byte[] fileData = _fileService.GetFile("C:\\Users\\EQProject\\Desktop\\Backgounds\\4688479.png");
            //string base64String = Convert.ToBase64String(fileData);

            //Dictionary<string, object> result = new Dictionary<string, object>();
            //result.Add("FileDataBase64", base64String);
            //return Ok(result);
        }

    }
}
