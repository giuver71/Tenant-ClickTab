using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using EQP.EFRepository.Core.Exceptions;
using EQP.EFRepository.Core.Helpers;
using EQP.EFRepository.Core.Models;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.EntityService.Generics;
using ClickTab.Web.Mappings;
using ClickTab.Web.Mappings.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClickTab.Core.HelperService;
using Autofac.Core;
using ClickTab.Web.Mappings.ModelsDTO.Generics;

namespace ClickTab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private AutoMappingService _autoMappingService;
        private UrlTokenService _urlTokenService;
        private EmailService _emailService;
        private UserRoleService _userRoleService;
        private JwtService _jwtService;



        public UserController(UserService userService, AutoMappingService autoMappingService, UrlTokenService urlTokenService,
            EmailService emailService,UserRoleService userRoleService, JwtService jwtService)
        {
            _userService = userService;
            _autoMappingService = autoMappingService;
            _urlTokenService = urlTokenService;
            _emailService = emailService;
            _jwtService = jwtService;
            _userRoleService = userRoleService;

        }


        [HttpGet, Route("/api/[controller]/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = _userService.GetAll().ToList();
            List<UserDTO> userDto = _autoMappingService.CurrentMapper.Map<List<UserDTO>>(users);
            return Ok(userDto);
        }

        [HttpGet, Route("/api/[controller]/GetAllFull")]
        public async Task<IActionResult> GetAllFull()
        {
            List<User> all = _userService.GetAllFull();
            List<UserDTO> userDto = _autoMappingService.CurrentMapper.Map<List<UserDTO>>(all);
            return Ok(userDto);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            User user = _userService.Get(id);
            UserDTO dtoUser = _autoMappingService.CurrentMapper.Map<UserDTO>(user);
            return Ok(dtoUser);
        }

        [HttpGet, Route("/api/[controller]/full/{id}")]
        public async Task<IActionResult> GetIDFullUser(int id)
        {
            var userToSave = _userService.GetIDFull(id);
            var userSavedID = _autoMappingService.CurrentMapper.Map<UserDTO>(userToSave);
            return Ok(userSavedID);
        }


        [HttpPost, Route("/api/[controller]")]
        public async Task<IActionResult> SaveUser([FromBody] UserDTO userDto)
        {
            User userToSave = _autoMappingService.CurrentMapper.Map<User>(userDto);
            int userSavedID = _userService.Save(userToSave);
            return Ok(userSavedID);
        }

        [HttpDelete, Route("/api/[controller]/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _userService.Delete(id);
            return Ok();
        }



        /// <summary>
        /// Metodo utilizzato per il cambio password al primo Login
        /// </summary>
        /// <param name="password"></param>
        /// <remarks> Nell'oggetto data di tipo Dynamic l'oggetto avrà le proprietà ID, Password, OldPassword</remarks>
        /// <returns></returns>
        [HttpPost, Route("/api/[controller]/FirstLoginResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> FirstLoginResetPassword([FromBody] dynamic data)
        {
            User currentUser = new User();
            int outValue;

            int? fk_user = int.TryParse(Convert.ToString(data.ID), out outValue) ? (int?)outValue : null;
            string newPassword = data.NewPassword?.ToString() ?? null;
            string oldPassword = data.OldPassword?.ToString() ?? null;
            string email = data.Email?.ToString() ?? null;


            if (fk_user != null)
            {
                currentUser = _userService.Get((int)fk_user);
            }
            else
            {
                currentUser = _userService.GetBy(x => x.Email == email).FirstOrDefault();
            }

            if (currentUser != null)
            {
                _userService.FirstLoginResetPassword(currentUser, newPassword, oldPassword);
                return Ok();
            }
            else
            {
                throw new EntityValidationException("L'email inserita non è presente nel sistema");
            }

        }




        /// <summary>
        /// API che crea un Token temporaneo per abilitare il cambio password per l'utente che lo richiede
        /// Il metodo invia una email con il link che contiene il token nell'url
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, Route("/api/[controller]/EnableResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> EnableResetPassword([FromBody] dynamic data)
        {
            User currentUser = new User();
            string email = data.Email?.ToString() ?? null;

            if (email != null)
            {
                currentUser = _userService.GetBy(a => a.Email == email).FirstOrDefault();
                if (currentUser != null)
                {
                    string _token = _urlTokenService.GenerateToken(currentUser);
                    _emailService.ResetPassword(currentUser, _token);
                }
                else
                {
                    throw new EntityValidationException("L'email inserita non è presente nel sistema");
                }
            }
            else
            {
                throw new Exception("Si è verificato un errore, riprova più tardi");
            }

            return Ok();
        }


        /// <summary>
        /// API che controlla se il token usato è esistente e ancola valido
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost, Route("/api/[controller]/CheckToken")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckToken([FromBody] dynamic data)
        {
            string token = data.Token?.ToString() ?? null;

            if (token != null)
            {
                UrlToken _token = _urlTokenService.GetBy(a => a.Token == token && a.DtmExpiration >= DateTime.Now).FirstOrDefault();
                if (_token != null)
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Codice non valido o scaduto, richiedine un altro!");
                }
            }
            else
            {
                throw new Exception("Codice non valido o scaduto, richiedine un altro!");
            }
        }

        /// <summary>
        /// API che reimposta la password all'utente controllando inoltre che il token esista e che sia associato alla email per cui si vuole 
        /// reimpostare la password
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="EntityValidationException"></exception>
        [HttpPost, Route("/api/[controller]/ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] dynamic data)
        {
            User currentUser = new User();
            string newPassword = data.Password?.ToString() ?? null;
            string email = data.Email?.ToString() ?? null;
            string token = data.Token?.ToString() ?? null;

            if (email != null && newPassword != null && token != null)
            {
                currentUser = _userService.GetBy(x => x.Email == email).FirstOrDefault();
                if (currentUser == null)
                {
                    throw new Exception("Codice non valido o scaduto, richiedine un altro!");
                }

                UrlToken _token = _urlTokenService.GetBy(a => a.Token == token && a.DtmExpiration >= DateTime.Now && a.FK_User == currentUser.ID).FirstOrDefault();
                if (_token != null)
                {

                    User user = _userService.ResetPassword(currentUser, newPassword);
                    _emailService.SendChangedPassword(user);
                    _urlTokenService.Delete(_token);

                    return Ok();
                }
                else
                {
                    throw new Exception("Codice non valido o scaduto, richiedine un altro!");
                }
            }
            else
            {
                throw new EntityValidationException("Si è verificato un errore, riprovare più tardi");
            }
        }


        [HttpGet("/api/[controller]/getAllRolesUserFacilityHashed/{FK_User}")]
        public async Task<IActionResult> GetAllRolesUserFacilityHashed(int FK_User)
        {
            List<string> hashedUserRoleList = new List<string>();

            List<UserRole> userRoles = _userRoleService.GetFullRoles(FK_User);
            List<Role> Roles = userRoles.Select(x => x.Role).ToList();
            List<RoleDTO> RolesDTO = _autoMappingService.CurrentMapper.Map<List<RoleDTO>>(Roles);
            foreach (var role in RolesDTO)
            {
                Dictionary<string, object> payload = new Dictionary<string, object>();
                // il PAYLOAD_ROLE_KEY è il nome dell'oggetto json che si legge quando si decodifica lato client
                payload.Add(AuthService.PAYLOAD_ROLE_KEY, role);
                string tokenrole = _jwtService.PayloadToToken(payload);
                hashedUserRoleList.Add(tokenrole);
            }
            return Ok(hashedUserRoleList);
        }

        [HttpGet("/api/[controller]/ChangeStatus/{status}/{id}")]
        public async Task<IActionResult> ChangeStatus(UserStatusEnum status,int id)
        {
            User user =new User();
            user.ID = id;
            user.Status = status;
            _userService.UpdateEntityProperties(user, true, new string[] { "Status" });
            return Ok();
        }
    }
}
