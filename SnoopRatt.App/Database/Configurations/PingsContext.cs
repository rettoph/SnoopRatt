using SnoopRatt.App.Database.ValueConverters;
using SnoopRatt.App.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Database.Configuration
{
    internal sealed class RoleMentionsConfiguration : IEntityTypeConfiguration<Ping>
    {
        public void Configure(EntityTypeBuilder<Ping> builder)
        {
            builder.HasKey(e => e.MessageId);
            builder.HasIndex(e => e.TimeStamp);
        }
    }
}
