using ClickTab.Core.DAL.Models.NotificationCenter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.NotificationCenter
{
    class NotificationDetailConfigurations : IEntityTypeConfiguration<NotificationDetail>
    {
        public void Configure(EntityTypeBuilder<NotificationDetail> builder)
        {
            builder.HasOne(e => e.Notification).WithMany(n => n.NotificationDetails).HasForeignKey(e => e.FK_Notification).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Receiver).WithMany().HasForeignKey(e => e.FK_Receiver).OnDelete(DeleteBehavior.Cascade);


            builder.HasCheckConstraint("CK_RECEIVER", @"(FK_Receiver IS NOT NULL) OR (FK_Receiver IS NULL AND ReceiverEmail IS NOT NULL)");
        }
    }
}
