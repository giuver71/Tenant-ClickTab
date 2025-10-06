using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClickTab.Core.DAL.Models.Generics;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Generics
{
    public class UpdateXlsConfigurations : IEntityTypeConfiguration<UpdateXls>
    {
        public void Configure(EntityTypeBuilder<UpdateXls> builder)
        {
            //REQUIRED
            builder.Property(p => p.FileName).IsRequired();
            builder.Property(p => p.LastUpdate).IsRequired();
            builder.HasIndex(p=>p.FileName ).IsUnique();


        }
    }
}
