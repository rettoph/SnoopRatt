using SnoopRatt.App.Database;
using SnoopRatt.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    public class GuildSettingsService
    {
        private MaryJaneContext _context;

        public GuildSettingsService(MaryJaneContext context)
        {
            _context = context;
        }

        public async Task SetRoleAsync(ulong guild, ulong role)
        {
            var settings = this.Get(guild);
            settings.Role = role;

            await _context.SaveChangesAsync();
        }

        public async Task SetTimezoneAsync(ulong guild, TimeZoneInfo timezone)
        {
            var settings = this.Get(guild);
            settings.TimeZone = timezone;

            await _context.SaveChangesAsync();
        }

        public GuildSettings Get(ulong guild)
        {
            var settings = _context.GuildSettings.FirstOrDefault(x => x.Id == guild);

            if (settings is null)
            {
                settings = new GuildSettings()
                {
                    Id = guild
                };

                _context.GuildSettings.Add(settings);
            }

            return settings;
        }
    }
}
