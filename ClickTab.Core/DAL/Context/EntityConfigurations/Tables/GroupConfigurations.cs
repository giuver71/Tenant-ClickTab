using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Models.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Tables
{
    public class GroupConfigurations : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            //REQUIRED
            builder.Property(u => u.Description).IsRequired();
            builder.Property(u => u.Code).IsRequired();

        }
    }
}
