using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Models.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Tables
{
    public class VatConfigurations : IEntityTypeConfiguration<Vat>
    {
        public void Configure(EntityTypeBuilder<Vat> builder)
        {
            builder.HasIndex(p => p.Code).IsUnique();
            //REQUIRED
            builder.Property(u => u.Description).IsRequired();
            builder.Property(u => u.Code).IsRequired();

        }
    }
}
