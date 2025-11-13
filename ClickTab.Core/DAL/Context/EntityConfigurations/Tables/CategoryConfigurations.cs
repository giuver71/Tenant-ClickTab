using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Models.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Tables
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //REQUIRED
            builder.Property(u => u.Description).IsRequired();
            builder.Property(u => u.Code).IsRequired();

        }
    }
}
