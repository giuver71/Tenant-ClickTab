using System;

namespace ClickTab.Web.Mappings.ModelsDTO.NotificationCenter
{
    public class NotificationDTO
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string AdditionalParams { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
