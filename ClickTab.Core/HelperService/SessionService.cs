using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using EQP.EFRepository.Core.Helpers;
using EQP.EFRepository.Core.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public class SessionService
    {
        public static readonly string SESSION_KEY = "CurrentSession";

        private HttpContext _currentContext;
        private DatabaseContext _dbContext;

        public SessionService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SetCurrentContext(HttpContext httpContext)
        {
            _currentContext = httpContext;
        }

        public SessionModel GetCurrentSession()
        {
            SessionModel currentSession = new SessionModel(null);
            byte[] sessionValue = null;
            bool sessionExist = _currentContext != null ? _currentContext.Session.TryGetValue(SESSION_KEY, out sessionValue) : false;

            if (!sessionExist)
                return currentSession;
            else
            {
                string serializedSession = Encoding.ASCII.GetString(sessionValue);
                currentSession = JsonConvert.DeserializeObject<SessionModel>(serializedSession, new JsonSerializerSettings()
                { TypeNameHandling = TypeNameHandling.Auto });

                return currentSession;
            }
        }

        public bool SessionAvailable()
        {
            bool result = true;
            try
            {
                byte[] sessionValue = null;
                bool sessionExist = _currentContext != null ? _currentContext.Session.TryGetValue(SESSION_KEY, out sessionValue) : false;
                result = sessionExist;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public void SetCurrentSession(User user)
        {
            if (_currentContext == null)
                return;

            SessionModel currentSession = new SessionModel(user);
            string serializedSession = JsonConvert.SerializeObject(currentSession, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            byte[] binaryObject = Encoding.ASCII.GetBytes(serializedSession);

            _currentContext.Session.Set(SESSION_KEY, binaryObject);

            //Setta i dati di sessione anche nell'helper di EFRepository in modo da poter gestire l'eventuale Audit e/o SoftDelete
            _dbContext.User = currentSession.User;
            _dbContext.UserString = currentSession.UserString;
            _dbContext.Language = currentSession.Language;
        }

    }

    [Serializable]
    public class SessionModel : ISessionModel
    {

        public SessionModel(IBaseEntity user)
        {
            this.User = user;
        }

        public IBaseEntity User { get; set; }
        public IBaseEntity Language { get; set; }
        public IBaseStringEntity UserString { get; set; }
    }
}
