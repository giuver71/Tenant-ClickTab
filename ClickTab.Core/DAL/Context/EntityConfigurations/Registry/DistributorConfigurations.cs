using ClickTab.Core.DAL.Models.Registry;
using ClickTab.Core.DAL.Models.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Context.EntityConfigurations.Registry
{
    public class DistributorConfigurations : IEntityTypeConfiguration<Distributor>
    {
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            //REQUIRED
            builder.Property(u => u.Description).IsRequired();

        }
    }
}
