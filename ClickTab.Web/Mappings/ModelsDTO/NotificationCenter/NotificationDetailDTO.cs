using System;

namespace ClickTab.Web.Mappings.ModelsDTO.NotificationCenter
{
    public class NotificationDetailDTO
    {
        public int ID { get; set; }
        public int FK_Notification { get; set; }
        public NotificationDTO Notification { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsRead { get; set; }
        public int? FK_User { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
