using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Models.NotificationCenter;
using ClickTab.Core.DAL.Repository.NotificationCenter;
using ClickTab.Core.EntityService.Generics;
using ClickTab.Core.HelperClass.NotificationCenter;
using ClickTab.Core.HelperService;
using ClickTab.Core.HelperService.NotificationCenter;
using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ClickTab.Core.EntityService.NotificationCenter
{
    public class NotificationService : IdentityService<NotificationRepository, Notification>
    {
        private SessionService _sessionService;
        private EmailService _emailService;
        private ConfigurationService _configurationService;
        private UserService _userService;
        private NotificationHubService _notificationHubService;
        public NotificationService(UnitOfWork<DatabaseContext> uow, SessionService sessionService, EmailService emailService, ConfigurationService configurationService, UserService userService, NotificationHubService notificationHubService) : base(uow)
        {
            _sessionService = sessionService;
            _emailService = emailService;
            _configurationService = configurationService;
            _userService = userService;
            _notificationHubService = notificationHubService;
        }

        /// <summary>
        /// Metodo per creare una notifica di prova che viene salvata ed inviata a tutti gli utenti.
        /// QUESTO METODO E TUTTI I SUOI RIFERIMETI POSSONO ESSERE RIMOSSI.
        /// </summary>
        public void CreateAndSendTestNotification()
        {
            try
            {
                _uow.BeginTransaction();
                    
                /* LOGICA DEL METODO */
        
                // Recupero i destinatari della notifica
                List<User> users = _userService.GetAll();

                // Creo il dizionario per rimpiazzare i placeholder
                Dictionary<string, object> additionalParams = new Dictionary<string, object>();
                additionalParams.Add("placeholderTest", "rimpiazzati");
                additionalParams.Add("placeholderTitle", "TITOLO");

                // Invoco il metodo per creare/inviare la notifica IN TRANSACTION con la action CREATE
                CreateOrSendNotifications(users, NotificationsEnum.SAMPLE_NOTIFICATION, additionalParams, NotificationActionEnum.CREATE);
                
                _uow.CommitTransaction();

                // Invoco il metodo per creare/inviare la notifica FUORI TRANSACTION con la action SEND
                CreateOrSendNotifications(users, NotificationsEnum.SAMPLE_NOTIFICATION, additionalParams, NotificationActionEnum.SEND);
            }
            catch (Exception ex)
            {
                if (_uow.GetActiveTransaction() != null)
                    _uow.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// In base all'azione passata gestisce la creazione della notifica come entità sul DB o l'invio di mail e notifiche push.
        /// </summary>
        /// <param name="userList">Lista degli User che devono ricevere la notifica (che sia solo email o anche push)</param>
        /// <param name="translationKey">Valore dell'enumeratore NotificationTranslationKeyEnum che rappresenta la notifica da salvare/inviare.</param>
        /// <param name="additionalParams">La stringa deve essere un json parsato contenente i valori di eventuali placeholder all'interno dei testi della notifica</param>
        /// <param name="action">Enumeratore che identifica l'azione da eseguire (invio o creazione della notifica in base alle Person passate)</param>
        public void CreateOrSendNotifications(List<User> userList, NotificationsEnum translationKey, Dictionary<string, object> additionalParams, NotificationActionEnum action)
        {
            //si fa la distinct degli utenti per non inviare la stessa notifica agi utenti che hanno più ruoli
            userList = userList.GroupBy(u => u.ID).Select(g => g.First()).ToList();

            // Recupera i dati della notifica: chiave del titolo, chiave del messaggio e testo di titolo/messaggio tradotto.
            // Se non viene passato il codice di una lingua viene presa la traduzione in italiano (con codice "it").
            NotificationData notificationData = NotificationHelperStaticService.GetNotificationData(translationKey);

            switch (action)
            {
                case NotificationActionEnum.CREATE:
                    // Crea la notifica con le relative NotificationDetail
                    CreateSystemNotification(userList, notificationData, additionalParams);
                    break;
                case NotificationActionEnum.SEND:
                    // Invia le mail e le notifiche push
                    SendNotificationsAndEmails(userList, notificationData, additionalParams);
                    break;
            }
        }

        /// <summary>
        /// Crea l'entità Notification popolando titolo e messaggio in base ai NotificationData ricevuti mentre
        /// ciclando gli User ricevuti crea le relative NotificationDetail.
        /// </summary>
        /// <remarks>
        /// Di default sul DB vengono salvate le chiavi per recueprare i testi di titolo e messaggio dai json di traduzione.
        /// </remarks>
        /// <param name="userList">Lista degli User che devono ricevere la notifica</param>
        /// <param name="notificationData">Dati della notifica da creare</param>
        /// <param name="additionalParams">La stringa deve essere un json parsato contenente i valori di eventuali placeholder all'interno dei testi della notifica</param>
        public void CreateSystemNotification(List<User> userList, NotificationData notificationData, Dictionary<string, object> additionalParams)
        {
            string AdditionalParamsString = JsonConvert.SerializeObject(additionalParams);

            // Creo la Notification con i dati ricevuti
            Notification notification = new Notification()
            {
                CreationDate = DateTime.Now,
                AdditionalParams = AdditionalParamsString,
                Title = notificationData.TitleKey,
                Message = notificationData.MessageKey,
            };

            // Filtro le Person con uno User, le ciclo e creo le NotificationDetail utilizzando ID e Email
            userList.ForEach(user =>
            {
                notification.NotificationDetails.Add(new NotificationDetail()
                {
                    FK_Notification = notification.ID,
                    SendDate = DateTime.Now,
                    FK_Receiver = user.ID,
                    ReceiverEmail = user.Email
                });
            });

            base.Save(notification);
        }


        /// <summary>
        /// Invia la notifica ai client connessi tramite web socket e invia anche una mail con lo stesso titolo/messaggio.
        /// </summary>
        /// <param name="userList">Lista degli User destinatari della notifica</param>
        /// <param name="notificationData">Dati della notifica da inviare</param>
        /// <param name="additionalParams">Parametri da usare per la sostituzione dei placeholders nei testi della notifica</param>
        public void SendNotificationsAndEmails(List<User> userList, NotificationData notificationData, Dictionary<string, object> additionalParams)
        {
            // Rimpiazzo i placeholders all'interno dei testi della notifica (sia del titolo che del messaggio)
            notificationData.Title = UtilityService.ReplaceTextPlaceholders(notificationData.Title, additionalParams);
            notificationData.Message = UtilityService.ReplaceTextPlaceholders(notificationData.Message, additionalParams);

            // Per gli utenti che non hanno specificato una mail per le notiiche queste vengono inviate alla mail specificata per la login.
            // Loris 30/11/2021: Aggiungo un filtro per togliere gli utenti con il flag "DisableMailNotifications" a true (non ricevono email ma solo la notifica push)
            foreach (User receiver in userList)
            {
                try
                {
                    _emailService.SystemNotificationEmail(receiver, notificationData.Title, notificationData.Message, additionalParams);
                }
                catch (Exception ex) { }
            }

            // Recupera dalle Person che hanno uno User associato il relativo ID
            List<int> userIDs = userList.Select(u => u.ID).ToList();
            // Invia la notifica ai client connessi.
            _notificationHubService.NotificationDataUpdate(userIDs, notificationData.Title);

            // La notifica può essere inviata anche su un thread parallelo, ecco un esempio di come fare:
            //_notificationHubService.SendNotificationOnParallelThread("NotificationDataUpdate", userIDs, notificationData.Title);
        }


        #region Override dei metodi

        // Override fatto per rimpiazzare le chiavi dei testi delle notifiche recuperate con i testi effettivi (sostituendo anche gli eventuali placeholders)
        public override Notification Get(int ID, params Expression<Func<Notification, object>>[] includes)
        {
            Notification notification = base.Get(ID, includes);
            notification = NotificationHelperStaticService.ReplaceNotificationTexts(notification);
            return notification;
        }

        // Override fatto per rimpiazzare le chiavi dei testi delle notifiche recuperate con i testi effettivi (sostituendo anche gli eventuali placeholders)
        public override List<Notification> GetBy(Expression<Func<Notification, bool>> predicate, params Expression<Func<Notification, object>>[] includes)
        {
            List<Notification> notificationList = base.GetBy(predicate, includes);
            for (int i = 0; i < notificationList.Count; i++)
                notificationList[i] = NotificationHelperStaticService.ReplaceNotificationTexts(notificationList[i]);
            return notificationList;
        }

        // Override fatto per rimpiazzare le chiavi dei testi delle notifiche recuperate con i testi effettivi (sostituendo anche gli eventuali placeholders)
        public override List<Notification> GetAll(params Expression<Func<Notification, object>>[] includes)
        {
            List<Notification> notificationList = base.GetAll(includes);
            for (int i = 0; i < notificationList.Count; i++)
                notificationList[i] = NotificationHelperStaticService.ReplaceNotificationTexts(notificationList[i]);
            return notificationList;
        }

        #endregion
    }
}
