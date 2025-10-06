using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.NotificationCenter;
using ClickTab.Core.DAL.Repository.NotificationCenter;
using ClickTab.Core.HelperService.NotificationCenter;
using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ClickTab.Core.EntityService.NotificationCenter
{
    public class NotificationDetailService : IdentityService<NotificationDetailRepository, NotificationDetail>
    {
        public NotificationDetailService(UnitOfWork<DatabaseContext> uow) : base(uow)
        {
        }

        public void MarkAsRead(int id)
        {
            NotificationDetail notificationDetail = base.Get(id);
            notificationDetail.ReadDate = DateTime.Now;
            base.Save(notificationDetail, true, true, false);
        }

        #region Override dei metodi

        // Override fatto per rimpiazzare le chiavi dei testi delle notifiche recuperate con i testi effettivi (sostituendo anche gli eventuali placeholders)
        public override NotificationDetail Get(int ID, params Expression<Func<NotificationDetail, object>>[] includes)
        {
            NotificationDetail notificationDetail = base.Get(ID, includes);
            if (notificationDetail.Notification != null)
                notificationDetail.Notification = NotificationHelperStaticService.ReplaceNotificationTexts(notificationDetail.Notification);
            return notificationDetail;
        }

        // Override fatto per rimpiazzare le chiavi dei testi delle notifiche recuperate con i testi effettivi (sostituendo anche gli eventuali placeholders)
        public override List<NotificationDetail> GetBy(Expression<Func<NotificationDetail, bool>> predicate, params Expression<Func<NotificationDetail, object>>[] includes)
        {
            List<NotificationDetail> notificationDetailList = base.GetBy(predicate, includes);
            for (int i = 0; i < notificationDetailList.Count; i++)
                if (notificationDetailList[i].Notification != null)
                    notificationDetailList[i].Notification = NotificationHelperStaticService.ReplaceNotificationTexts(notificationDetailList[i].Notification);
            return notificationDetailList;
        }

        // Override fatto per rimpiazzare le chiavi dei testi delle notifiche recuperate con i testi effettivi (sostituendo anche gli eventuali placeholders)
        public override List<NotificationDetail> GetAll(params Expression<Func<NotificationDetail, object>>[] includes)
        {
            List<NotificationDetail> notificationDetailList = base.GetAll(includes);
            for (int i = 0; i < notificationDetailList.Count; i++)
                if (notificationDetailList[i].Notification != null)
                    notificationDetailList[i].Notification = NotificationHelperStaticService.ReplaceNotificationTexts(notificationDetailList[i].Notification);
            return notificationDetailList;
        }

        #endregion
    }
}
