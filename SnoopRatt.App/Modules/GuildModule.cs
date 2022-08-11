using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SnoopRatt.App.Services;
using SnoopRatt.App.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Modules
{

    [RequireContext(ContextType.Guild)]
    public class GuildModule : ModuleBase<SocketCommandContext>
    {
        private GuildService _guilds;
        private UserService _users;
        private GuildSettingsService _settings;
        private PaginatorService _paginators;

        public GuildModule(GuildService guilds, UserService users, GuildSettingsService settings, PaginatorService paginators)
        {
            _guilds = guilds;
            _settings = settings;
            _paginators = paginators;
            _users = users;
        }

        [Command("ranks")]
        public async Task Ranks()
        {
            await _paginators.Add(new GuildPaginator(this.Context.Guild, _users.All(true).ToList()), await this.ReplyAsync("..."));
        }

        [Command("settings")]
        public async Task Settings()
        {
            var settings = _settings.Get(this.Context.Guild.Id);

            var embed = new EmbedBuilder();

            embed.AddField("Role", $"<@&{settings.Role}>");
            embed.AddField("Timezone", settings.TimeZone.DisplayName);

            await this.ReplyAsync(embed: embed.Build(), allowedMentions: AllowedMentions.None);
        }

        [Command("get timezones")]
        [Alias("gtz")]
        public async Task GetTimeZones()
        {
            await _paginators.Create<TimeZonePaginator>(await this.ReplyAsync("..."));
        }

        [Command("get time")]
        [Alias("gt")]
        public Task GetTime()
        {
            var timezone = _settings.Get(this.Context.Guild.Id).TimeZone;
            var time = TimeZoneInfo.ConvertTimeFromUtc(this.Context.Message.Timestamp.UtcDateTime, timezone);

            this.ReplyAsync($"Message recieved at {time}");

            return Task.CompletedTask;
        }

        [Command("set timezone")]
        [Alias("stz")]
        public async Task SetTimeZone([Remainder]  string id)
        {
            TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(id);
            
            await _settings.SetTimezoneAsync(this.Context.Guild.Id, timezone);
            await this.Context.Message.AddReactionAsync(Emoji.Parse("✅"));
        }

        [Command("set role")]
        [Alias("sr")]
        public async Task SetRole(IRole role)
        {
            if(!role.IsMentionable)
            {
                await this.ReplyAsync("Unmentionable role given.");
            }

            await _settings.SetRoleAsync(this.Context.Guild.Id, role.Id);
            await this.Context.Message.AddReactionAsync(Emoji.Parse("✅"));
        }

        [Command("refresh")]
        [Alias("r")]
        public async Task Refresh(SocketTextChannel channel, ulong from)
        {
            await _guilds.Refresh(this.Context.Guild.Id, channel, from, await this.ReplyAsync("Scanning..."));
        }
    }
}
