using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Exceptions;
using EQP.EFRepository.Core.Interface;
using EQP.EFRepository.Core.Services;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Repository;
using ClickTab.Core.DAL.Repository.Generics;
using ClickTab.Core.HelperService;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ClickTab.Core.EntityService.Generics
{
    public class UserService : IdentityService<UserRepository, User>
    {

        private PasswordService _passwordService;
        private EmailService _emailService;
        private DatabaseContext _ctx;

        public UserService(UnitOfWork<DatabaseContext> uow, PasswordService passwordService, EmailService emailService, DatabaseContext ctx) : base(uow)
        {
            _passwordService = passwordService;
            _emailService = emailService;
            _ctx=ctx;
        }

        /// <summary>
        /// Funzione che predispone l'utenza di admin all'interno della tabella Users.
        /// E' utilizzata quando viene creata una nuova istanza del DB
        /// </summary>
        public void CreateAdminUser()
        {
            string email = "admin@clicktab.it";
            if (Exists(p=>p.Email==email))
            {
                return;
            }
            //Utenza di admin con password = "admin"
            User adminUser = new User()
            {
                Name = "Admin",
                Surname = "Admin",
                Email = email,
                Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                ChangedPassword = true,
                SubscriptionDate = DateTime.Now
            };
            base.Save(adminUser);
        }


        #region Override metodi


        // *********************************************************************************************************
        // Cancellare questo metodo per avere i dati veri dal DB
        // *********************************************************************************************************
      





        protected override void SaveValidation(User entity)
        {
            if (Exists(x => x.Email == entity.Email && x.ID != entity.ID))
                throw new EntityValidationException("E' già presente un utente registrato nel sistema con questa email");
        }


        /// <summary>
        /// Durante il salvataggio, nella stessa transazione, mi assicuro che una volta salvato l'utente e ricavato L'ID
        /// abbia tutti i dati necessari per ricostruire la lista di oggetti UserCompanyRole da salvare nell'apposito servizio 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveChanges"></param>
        /// <param name="IgnoreValidation"></param>
        /// <returns></returns>
        public override int Save(User entity, bool saveChanges = true, bool IgnoreValidation = false, bool checkConcurrency = true, bool fullAudit = false)
        {
            try
            {
                bool sendMail = (entity.ID == 0);
                string clearPassword = null;
                if (entity.ID == 0)
                {
                    entity.SubscriptionDate = DateTime.Now;

                    // creo una password casuale se non mi arriva nel Json
                    clearPassword = string.IsNullOrEmpty(entity.Password) ? _passwordService.MakePassword() : entity.Password;
                    // eseguo l'encrypt della password
                    entity.Password = _passwordService.EncryptSHA256(clearPassword);
                }

                int userSavedID = base.Save(entity, saveChanges);

                if (sendMail == true)
                    _emailService.CreateNewCredentialEmail(entity, clearPassword);

                return userSavedID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Viene gestita la reimpostazione della password, al primo accesso dell'utente
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="oldPassword"></param>
        public void FirstLoginResetPassword(User user, string newPassword, string oldPassword)
        {

            string decryptedPassword;

            try
            {
                string tempEncryptedPassword = _passwordService.EncryptSHA256(oldPassword);

                if (tempEncryptedPassword != user.Password)
                    throw new EntityValidationException("La vecchia password inserita non corrisponde a quella attuale, riprovare!");

                user.ChangedPassword = true;
                decryptedPassword = newPassword;

                user.Password = _passwordService.EncryptSHA256(decryptedPassword);
                base.Save(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        /// <summary>
        /// Metodo che aggiorna la password per uno User, la password viene decisa dall'utente stesso
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User ResetPassword(User user, string password)
        {

            string decryptedPassword;

            try
            {
                _uow.BeginTransaction();

                user.Password = _passwordService.EncryptSHA256(password);
                base.Save(user);
                _uow.SaveChanges();

                _uow.CommitTransaction();
                return user;

            }
            catch (Exception ex)
            {
                _uow.RollbackTransaction();
                throw ex;
            }
        }

        #endregion

        // <ewz:Include>
        public List<User> GetAllFull()
        {
            return _repository.GetAllFull();
        }

        public User GetFull(int ID)
        {
            return _repository.GetFull(ID);
        }
    }
}
