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
    internal sealed class GuildSettingsConfiguration : IEntityTypeConfiguration<GuildSettings>
    {
        public void Configure(EntityTypeBuilder<GuildSettings> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.TimeZone)
                .HasConversion<TimeZoneInfoValueConverter>();
        }
    }
}
