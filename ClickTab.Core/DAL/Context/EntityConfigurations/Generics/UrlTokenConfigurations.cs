using ClickTab.Core.DAL.Models.Generics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Generics
{
    public class UrlTokenConfigurations : IEntityTypeConfiguration<UrlToken>
    {
        public void Configure(EntityTypeBuilder<UrlToken> builder)
        {
            builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.FK_User).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
