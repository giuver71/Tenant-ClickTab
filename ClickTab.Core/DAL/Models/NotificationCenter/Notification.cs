using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.DAL.Models.NotificationCenter
{
    public class Notification : IBaseEntity
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string AdditionalParams { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public List<NotificationDetail> NotificationDetails { get; set; } = new List<NotificationDetail>();
    }

    public enum NotificationActionEnum
    {
        CREATE = 1,
        SEND = 2
    }
}
