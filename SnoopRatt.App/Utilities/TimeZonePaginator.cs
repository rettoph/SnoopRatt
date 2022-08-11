using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Utilities
{
    public class TimeZonePaginator : Paginator
    {
        public static TimeZoneInfo[] Timezones = TimeZoneInfo.GetSystemTimeZones().OrderBy(x => x.Id).ToArray();

        public override int Max => Timezones.Length / this.Size;
        public int Size => 30;

        public override Task Load(int page, EmbedBuilder embed, List<FileAttachment> attachments)
        {
            var timezones = Timezones.Skip(this.Size * page).Take(this.Size).Select(x => x.Id);
            var values = string.Join('\n', timezones);

            embed.AddField("Timezones", values);

            return Task.CompletedTask;
        }
    }
}
