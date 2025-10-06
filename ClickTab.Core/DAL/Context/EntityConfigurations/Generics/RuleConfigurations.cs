using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickTab.Core.DAL.Models.Generics;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Generics
{
    public class RuleConfigurations : IEntityTypeConfiguration<Rule>
    {
        public void Configure(EntityTypeBuilder<Rule> builder)
        {
            builder.Property(u => u.DescriptionEnum).IsRequired();
        }
    }
}
