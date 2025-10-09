using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using ClickTab.Core.DAL.Models.Generics;

namespace ClickTab.Core.HelperService
{
    public class EmailService
    {
        private ConfigurationService _configurationService;
        private FileService _fileService;
        private const string _TEMPLATE_MAIL_NEW_USER = "NewUserTemplate.html";
        private const string _TEMPLATE_MAIL_RESET_PASSWORD = "ResetPasswordTemplate.html";
        private const string _TEMPLATE_MAIL_NEW_USER_WELCOME_IMAGE = "LogoClickTab.png";
        private const string _TEMPLATE_MAIL_RESET_PASSWORD_IMAGE = "LogoClickTab.png";
        public const string _TEMPLATE_MAIL_SYSTEM_NOTIFICATION = "SystemNotificationTemplate.html";
        private const string _TEMPLATE_MAIL_RESET_PASSWORD_URLTOKEN = "ResetPasswordUrlToken.html";
        private const string _TEMPLATE_MAIL_CHANGED_PASSWORD = "ChangedPassword.html";

        public EmailService(ConfigurationService configService, FileService fileService)
        {
            _configurationService = configService;
            _fileService = fileService;
        }


        /// <summary>
        /// Genera un oggetto MailMessage da usare per le mail di creazione di nuove utenze
        /// </summary>
        /// <param name="User">Utente a cui inviare la mail di creazione </param>
        /// <param name="ClearPassword"></param>
        /// <param name="Send">Se TRUE allora il messaggio viene anche inviato altrimenti viene solo restituito senza essere inviato</param>
        /// <returns></returns>
        public MailMessage CreateNewCredentialEmail(User User, string ClearPassword, bool Send = true, bool onParallelThread = true)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_configurationService.EmailFrom);
            mail.To.Add(User.Email);
            mail.Subject = $"{_configurationService.AppName} - Creazione nuova utenza";
            mail.IsBodyHtml = true;

            SetupImageEmail(mail, _TEMPLATE_MAIL_NEW_USER_WELCOME_IMAGE, out string imageContentID);

            //Recupera dalle risorse il template html da usare per la mail
            string mailMessage = _fileService.GetTemplateMail(_TEMPLATE_MAIL_NEW_USER);
            mailMessage = mailMessage.Replace("##IMAGE_URL##", imageContentID);
            mailMessage = mailMessage.Replace("##USER_NAME##", User.Name);
            mailMessage = mailMessage.Replace("##USER_SURNAME##", User.Surname);
            mailMessage = mailMessage.Replace("##PORTAL_URL##", _configurationService.BaseDomainClient);
            mailMessage = mailMessage.Replace("##PORTAL_NAME##", _configurationService.AppName);
            mailMessage = mailMessage.Replace("##USER_EMAIL##", User.Email);
            mailMessage = mailMessage.Replace("##USER_PASSWORD##", ClearPassword);

            mail.Body = mailMessage;

            if (Send == true)
                this.SendEmail(mail, onParallelThread);

            return mail;
        }

        /// <summary>
        /// Invia un email con il link che abilita al reset della password (Password dimenticata)
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="token"></param>
        public void ResetPassword(User currentUser, string token)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_configurationService.EmailFrom);
            mail.To.Add(currentUser.Email);
            mail.Subject = $"{_configurationService.AppName} - Reset password";
            mail.IsBodyHtml = true;


            string urlReset = $"{_configurationService.BaseDomainClient}/#/changeForgotPassword/{token}";

            //Recupera dalle risorse il template html da usare per la mail
            string mailMessage = _fileService.GetTemplateMail(_TEMPLATE_MAIL_RESET_PASSWORD);
            //mailMessage = mailMessage.Replace("##IMAGE_URL##", imageContentID);
            mailMessage = mailMessage.Replace("##USER_NAME##", currentUser.Name);
            mailMessage = mailMessage.Replace("##USER_SURNAME##", currentUser.Surname);
            mailMessage = mailMessage.Replace("##PORTAL_URL##", _configurationService.BaseDomainClient);
            mailMessage = mailMessage.Replace("##PORTAL_NAME##", _configurationService.AppName);
            mailMessage = mailMessage.Replace("##RESET_PASSWORD_URL##", urlReset);

            mail.Body = mailMessage;



            this.SendEmail(mail, false);
        }

        /// <summary>
        /// Recupera il template per inviare le mail delle notifiche di sistema.
        /// Una volta letto il template sostituisce i placeholder nel testo con i valori passati negli additionalParams.
        /// </summary>
        /// <param name="user">User destinatario della mail</param>
        /// <param name="notificationTitle">Titolo della notifica, viene usato come oggetto della mail</param>
        /// <param name="notificationMessage">Testo della notifica, viene inserito nel messaggio della mail</param>
        /// <param name="additionalParams">Dizionario contenente i valori da sostituire ai placeholder nel testo della notifica</param>
        /// <returns></returns>
        public MailMessage SystemNotificationEmail(User user, string notificationTitle, string notificationMessage, Dictionary<string, object> additionalParams)
        {
            string mailBody = _fileService.GetTemplateMail(_TEMPLATE_MAIL_SYSTEM_NOTIFICATION);
            mailBody = mailBody.Replace("{UserName}", user.Name);
            mailBody = mailBody.Replace("{UserSurname}", user.Surname);
            mailBody = mailBody.Replace("{BaseDomain}", _configurationService.BaseDomainClient);
            mailBody = mailBody.Replace("{NotificationMessage}", notificationMessage);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_configurationService.EmailFrom);
            mail.To.Add(user.Email);
            mail.Subject = notificationTitle;
            mail.IsBodyHtml = true;
            mail.Body = mailBody;
            this.SendEmail(mail, true);

            return mail;
        }


        /// <summary>
        /// Invia la mail passata come parametro
        /// </summary>
        /// <param name="message">Messaggio contenente tutte le informazioni riguardanti l'email da inviare</param>
        public void SendEmail(MailMessage message, bool onParallelThread)
        {
            message.IsBodyHtml = true;
            if (onParallelThread)
                SendMailOnParallelThread(message);
            else
                SendMailOnMainThread(message);

        }

        public void SendChangedPassword(User currentUser)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_configurationService.EmailFrom);
            mail.To.Add(currentUser.Email);
            mail.Subject = $"{_configurationService.AppName} - Password reimpostata con successo";
            mail.IsBodyHtml = true;

            //Recupera dalle risorse il template html da usare per la mail
            string mailMessage = _fileService.GetTemplateMail(_TEMPLATE_MAIL_CHANGED_PASSWORD);
            //mailMessage = mailMessage.Replace("##IMAGE_URL##", imageContentID);
            mailMessage = mailMessage.Replace("##USER_NAME##", currentUser.Name);
            mailMessage = mailMessage.Replace("##USER_SURNAME##", currentUser.Surname);
            mailMessage = mailMessage.Replace("##PORTAL_URL##", _configurationService.BaseDomainClient);
            mailMessage = mailMessage.Replace("##PORTAL_NAME##", _configurationService.AppName);

            mail.Body = mailMessage;



            this.SendEmail(mail, false);
        }




        #region Funzioni private

        /// <summary>
        /// Invio mail su un thread parallelo in modo asyncrono. Non influenza il ramo principale e questo non aspetta l'esito dell'invio.
        /// In caso di errore scrive il log e non genera eccezioni.
        /// </summary>
        /// <param name="message"></param>
        private void SendMailOnParallelThread(MailMessage message)
        {
            object sendMailParameters = new
            {
                configurationService = _configurationService,
                message = message
            };

            Thread sendMailThread = new Thread((object parameters) =>
            {
                ConfigurationService _config = parameters.GetType().GetProperty("configurationService").GetValue(parameters) as ConfigurationService;
                MailMessage _message = parameters.GetType().GetProperty("message").GetValue(parameters) as MailMessage;

                using (_message)
                {
                    using (SmtpClient smtpClient = new SmtpClient(_config.EmailSMTPClient, _config.EmailPortNumber))
                    {
                        NetworkCredential basicCredential = new NetworkCredential(_config.EmailFrom, _config.EmailPassword);
                        smtpClient.Credentials = basicCredential;
                        smtpClient.EnableSsl = _config.EmailEnableSSL.HasValue ? _config.EmailEnableSSL.Value : false;

                        try { smtpClient.Send(_message); }
                        catch (Exception ex) { ExceptionLogHelperService.CreateLog(ex); }
                    }
                }
            });

            sendMailThread.Start(sendMailParameters);
        }

        /// <summary>
        /// Invio mail sul thread principale in modo sincrono. Aspetta la risposta dell'smtp prima di concludere il processo
        /// e in caso di errore genera un'eccezione oltre a scrivere il log.
        /// </summary>
        /// <param name="message"></param>
        private void SendMailOnMainThread(MailMessage message)
        {
            using (message)
            {
                using (SmtpClient smtpClient = new SmtpClient(_configurationService.EmailSMTPClient, _configurationService.EmailPortNumber))
                {
                    NetworkCredential basicCredential = new NetworkCredential(_configurationService.EmailFrom, _configurationService.EmailPassword);
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = _configurationService.EmailEnableSSL.HasValue ? _configurationService.EmailEnableSSL.Value : false;

                    try
                    {
                        smtpClient.Send(message);
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogHelperService.CreateLog(ex);
                        throw ex;
                    }
                }
            }
        }

        private void SetupImageEmail(MailMessage mail, string imageFileName, out string imageContentID)
        {
            var contentID = "Image";
            string localImagePath = null;
            if (_configurationService.IsDevelopment == true)
                localImagePath = Path.Combine(Environment.CurrentDirectory, "ClickTabClient", "src", "assets", "img", "email", imageFileName);
            else
                localImagePath = Path.Combine(_configurationService.BaseDomainClient, "assets", "img", "email", imageFileName);

            var inlineLogo = new Attachment(localImagePath);
            inlineLogo.ContentId = contentID;
            inlineLogo.ContentDisposition.Inline = true;
            inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            imageContentID = inlineLogo.ContentId;

            mail.Attachments.Add(inlineLogo);
        }

        #endregion
    }
}
