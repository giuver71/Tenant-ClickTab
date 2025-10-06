using ClickTab.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.HelperClass.NotificationCenter
{
    public class NotificationData
    {
        public string TitleKey { get; set; }
        public string MessageKey { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Classe statica contenente il nome del nodo del json di traduzione in cui sono contenuti tutti i testi
    /// delle notifiche.
    /// </summary>
    public static class NotificationTranslationKeys
    {
        public const string Node = "NOTIFICATIONS.";
        public const string TitleChildNode = "TITLE";
        public const string MessageChildNode = "MESSAGE";
    }

    /// <summary>
    /// Enumeratore in cui censire tutte le notifiche. Ogni valore deve avere l'attributo "TranslationKey" in cui viene 
    /// specificato il nodo del json di traduzione contenente il testo della notifica stessa configurato come segue:
    /// [TranslationKey(TranslationKey = NotificationTranslation.Node + "SAMPLE_NOTIFICATION.")]
    /// </summary>
    public enum NotificationsEnum
    {
        [TranslationKey(TranslationKey = NotificationTranslationKeys.Node + "SAMPLE_NOTIFICATION.")]
        SAMPLE_NOTIFICATION = 1,
    }

}
