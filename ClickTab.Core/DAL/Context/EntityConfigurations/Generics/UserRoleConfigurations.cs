using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ClickTab.Core.DAL.Models.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Generics
{
    public class UserRoleConfigurations : IEntityTypeConfiguration<UserRole>
    {

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {

            builder.HasOne(u => u.User).WithMany(r => r.UserRoles).HasForeignKey(x => x.FK_User);
            builder.HasOne(u => u.Role).WithMany(r => r.UserRoles).HasForeignKey(x => x.FK_Role);
        }
    }
}
