using SnoopRatt.App.Constants;
using SnoopRatt.App.Database;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Entities;
using SnoopRatt.QuickChart;
using SnoopRatt.QuickChart.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    public class PingService
    {
        private MaryJaneContext _context;
        private GuildSettingsService _settings;
        private QuickChartService _quickChart;

        public PingService(MaryJaneContext context, GuildSettingsService settings, QuickChartService quickChart)
        {
            _context = context;
            _settings = settings;
            _quickChart = quickChart;
        }

        public async Task Set(ulong message, ulong guild, ulong channel, ulong user, DateTime dateTime)
        {
            var settings = _settings.Get(guild);
            var timestamp = TimeZoneInfo.ConvertTimeFromUtc(dateTime, settings.TimeZone);

            var mention = this.Get(message);
            mention.GuildId = guild;
            mention.ChannelId = channel;
            mention.UserId = user;
            mention.TimeStamp = timestamp;
            mention.Status = Ping.GetStatus(timestamp);
            mention.Period = Ping.GetPeriod(timestamp);

            await _context.SaveChangesAsync();
        }

        public Ping Get(ulong message)
        {
            var mention = _context.Pings.FirstOrDefault(x => x.MessageId == message);

            if (mention is null)
            {
                mention = new Ping()
                {
                    MessageId = message
                };

                _context.Pings.Add(mention);
            }

            return mention;
        }
    }
}
