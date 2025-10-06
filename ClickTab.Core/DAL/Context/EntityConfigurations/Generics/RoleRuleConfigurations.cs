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
    public class RoleRuleConfigurations : IEntityTypeConfiguration<RoleRule>
    {
        public void Configure(EntityTypeBuilder<RoleRule> builder)
        {
            builder.HasOne<Role>(sc => sc.Role).WithMany(c => c.RoleRules).HasForeignKey(sc => sc.FK_Role);
            builder.HasOne<Rule>(sc => sc.Rule).WithMany(c => c.RoleRules).HasForeignKey(sc => sc.FK_Rule);

            builder.Property(o => o.CanCreate).IsRequired();
            builder.Property(o => o.CanDelete).IsRequired();
            builder.Property(o => o.CanEdit).IsRequired();
        }

    }
}
