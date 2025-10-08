using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using ClickTab.Core.DAL.Models.Anagrafica;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.EntityService.Generics;
using ClickTab.Core.HelperService;
using ClickTab.Web.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClickTab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService _authService;
        private AutoMappingService _mappingService;
        private JwtService _jwtService;

        public AuthController(AuthService authService, AutoMappingService mappingService, JwtService jwtService)
        {
            _authService = authService;
            _mappingService = mappingService;
            _jwtService = jwtService;
            
        }

        [HttpPost, Route("/api/[controller]/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] dynamic loginData)
        {
            User loginUser = _authService.Login(loginData.Email.ToString(), loginData.Password.ToString());
            if (loginUser.Status==UserStatusEnum.Disabilitato)
            {
                throw new Exception("Utente non abilitato all'accesso");
            }
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add(AuthService.PAYLOAD_USER_KEY, _mappingService.CurrentMapper.Map<User>(loginUser));

            string token = _jwtService.PayloadToToken(payload);

            return new JsonResult(token);
        }
    }
}
