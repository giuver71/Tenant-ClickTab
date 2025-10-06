using ClickTab.Core.Attributes;
using ClickTab.Core.DAL.Models.NotificationCenter;
using ClickTab.Core.Extensions;
using ClickTab.Core.HelperClass.NotificationCenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClickTab.Core.HelperService.NotificationCenter
{
    public static class NotificationHelperStaticService
    {
        /// <summary>
        /// Recupera un oggetto NotificationData che contiene i dati della notifica rappresentata dall'enumeratore passato in input nella lingua scelta.
        /// </summary>
        /// <param name="translationKey">Valore dell'enumeratore che identifica la notifica da recuperare</param>
        /// <param name="languageCode">Codice della lingua nella quale si vuole recuperare la notifica. Di default "it" per recuperare quelle in italiano.</param>
        /// <returns>
        /// Restituisce un oggetto NotificationData con le seguenti proprietà:
        /// TitleKey: chiave per recuperare il titolo nel json di traduzione
        /// MessageKey: chiave per recuperare il messaggio nel json di traduzione
        /// Title: testo del titolo contenuto nel json
        /// Message: testo del messaggio contenuto nel json
        /// </returns>
        public static NotificationData GetNotificationData(NotificationsEnum translationKey, string languageCode = "it")
        {
            NotificationData result = new NotificationData();

            JObject translationsJSON = GetTranslationJson(languageCode);
            if (translationsJSON != null)
            {
                result.TitleKey = translationKey.GetAttribute<TranslationKeyAttribute>().TranslationKey + NotificationTranslationKeys.TitleChildNode;
                result.MessageKey = translationKey.GetAttribute<TranslationKeyAttribute>().TranslationKey + NotificationTranslationKeys.MessageChildNode;
                result.Title = translationsJSON.SelectToken(result.TitleKey) != null ? translationsJSON.SelectToken(result.TitleKey).ToString() : result.TitleKey;
                result.Message = translationsJSON.SelectToken(result.MessageKey) != null ? translationsJSON.SelectToken(result.MessageKey).ToString() : result.MessageKey;
            }
            return result;
        }

        /// <summary>
        /// A partire dall'oggetto Notification passato in input, se le proprietà Title e Message contengono le chiavi dei testi nei json di traduzione,
        /// recupera i testi completi della notifica e rimpiazza gli eventuali placeholders presenti nei testi.
        /// </summary>
        /// <param name="notification">Oggetto Notification del quale si vuole recuperare i testi completi</param>
        /// <param name="languageCode">Codice della lingua nella quale si vuole recuperare i testi. Di default vale "it" e recupera i testi in italiano.</param>
        /// <returns>Restituisce l'oggetto Notification passato in input dopo aver aggiornato i valori delle proprietà Title e Message.</returns>
        public static Notification ReplaceNotificationTexts(Notification notification, string languageCode = "it")
        {
            if (CheckIfNotificationHasTranslationKeys(notification))
            {
                JObject translationsJSON = GetTranslationJson(languageCode);
                if (translationsJSON != null)
                {
                    notification.Title = translationsJSON.SelectToken(notification.Title) != null ? translationsJSON.SelectToken(notification.Title, false).ToString() : notification.Title;
                    notification.Message = translationsJSON.SelectToken(notification.Message) != null ? translationsJSON.SelectToken(notification.Message, false).ToString() : notification.Message;
                }
            }

            Dictionary<string, object> additionalParams = !String.IsNullOrEmpty(notification.AdditionalParams) ? JsonConvert.DeserializeObject<Dictionary<string, object>>(notification.AdditionalParams) : new Dictionary<string, object>();
            notification.Title = UtilityService.ReplaceTextPlaceholders(notification.Title, additionalParams);
            notification.Message = UtilityService.ReplaceTextPlaceholders(notification.Message, additionalParams);

            return notification;
        }

        /// <summary>
        /// Recupera il json di traduzione in base alla lingua passata in input e lo restituisce sotto forma di JObject.
        /// </summary>
        /// <param name="languageCode">Codice della lingua di cui si vuole recuperare il json di traduzione (esempio: "it"). 
        /// Questo valore deve coincidere con il nome del json da recuerare nella cartella i18n.</param>
        /// <returns></returns>
        private static JObject GetTranslationJson(string languageCode)
        {
            string i18nString = null;

            string i18nPath = Path.Combine(Environment.CurrentDirectory, "i18n", languageCode.ToLower() + ".json");
            using (StreamReader reader = new StreamReader(i18nPath, Encoding.GetEncoding("iso-8859-1")))
                i18nString = reader.ReadToEnd();

            Dictionary<string, object> localizationJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(i18nString);
            JObject jsonObj = JObject.Parse(JsonConvert.SerializeObject(localizationJson));
            return jsonObj;
        }

        /// <summary>
        /// Controlla se nell'oggetto Notification passato in input le proprietà Title e Message contengono le chiavi di traduzione
        /// della notifica all'interno dei json di traduzione.
        /// </summary>
        /// <remarks>
        /// Viene controllato che le proprietà Title e Message non siano vuote/nulle, inizino con il nome del nodo delle notifiche e 
        /// terminino con il nome del nodo child di riferimento all'interno dei json di traduzione.
        /// </remarks>
        /// <param name="notification">Oggetto Notification da controllare</param>
        /// <returns>Restituisce TRUE se le prorpietà Title e Message dell'oggetto in input rappresentano un nodo nei json di traduzione, altrimenti FALSE</returns>
        private static bool CheckIfNotificationHasTranslationKeys(Notification notification)
        {
            return !String.IsNullOrEmpty(notification.Title) && notification.Title.StartsWith(NotificationTranslationKeys.Node) && notification.Title.EndsWith("." + NotificationTranslationKeys.TitleChildNode)
            && !String.IsNullOrEmpty(notification.Message) && notification.Message.StartsWith(NotificationTranslationKeys.Node) && notification.Message.EndsWith("." + NotificationTranslationKeys.MessageChildNode);
        }
    }
}
