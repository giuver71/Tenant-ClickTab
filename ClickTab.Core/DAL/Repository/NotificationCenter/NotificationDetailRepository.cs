using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.NotificationCenter;
using EQP.EFRepository.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.DAL.Repository.NotificationCenter
{
    public class NotificationDetailRepository : IdentityRepository<NotificationDetail>
    {
        public NotificationDetailRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
