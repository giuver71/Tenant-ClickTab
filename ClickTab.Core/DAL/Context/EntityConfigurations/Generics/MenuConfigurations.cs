using ClickTab.Core.DAL.Models.Generics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Generics
{
    public class MenuConfigurations : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            //REQUIRED
            builder.Property(u => u.Label).IsRequired();
            builder.Property(u => u.ClientCode).IsRequired();

            builder.HasOne(c => c.Parent).WithMany(c => c.Children).HasForeignKey(c => c.FK_Parent).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
