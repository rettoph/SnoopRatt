using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Entities
{
    public class GuildSettings
    {
        public ulong Id { get; set; }

        public ulong? Role { get; set; }

        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Local;
    }
}
