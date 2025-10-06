using ClickTab.Core.DAL.Models.Generics;
using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.DAL.Models.NotificationCenter
{
    public class NotificationDetail : IBaseEntity
    {
        public int ID { get; set; }
        public int FK_Notification { get; set; }
        public Notification Notification { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public int? FK_Receiver { get; set; }
        public User Receiver { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
