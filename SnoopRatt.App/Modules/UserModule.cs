using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Services;
using SnoopRatt.App.Utilities;
using SnoopRatt.QuickChart.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Modules
{
    [RequireContext(ContextType.Guild)]
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        private UserService _users;
        private GuildSettingsService _settings;
        private PaginatorService _paginator;
        private QuickChartService _quickCharts;

        public UserModule(QuickChartService quickCharts, PaginatorService paginator, UserService users, GuildSettingsService settings)
        {
            _users = users;
            _settings = settings;
            _paginator = paginator;
            _quickCharts = quickCharts;
        }

        [Command("enable alerts")]
        public async Task Delete(bool value = true)
        {
            await _users.SetAlertOnLevelUp(this.Context.User.Id, value);

            await this.Context.Message.AddReactionAsync(Emoji.Parse("✅"));
        }

        [Command("deletepings")]
        public async Task Delete(SocketUser user = null)
        {
            user ??= this.Context.User;

            if (user != this.Context.User)
            {
                var guildUser = this.Context.Guild.GetUser(this.Context.User.Id);

                if (!guildUser.GuildPermissions.Administrator)
                {
                    await this.Context.Message.AddReactionAsync(Emoji.Parse("❌"));

                    return;
                }
            }

            await _users.Delete(this.Context.User.Id);

            await this.Context.Message.AddReactionAsync(Emoji.Parse("✅"));
        }

        [Command("resetxp")]
        public async Task Reset(SocketUser user = null)
        {
            user ??= this.Context.User;

            if(user != this.Context.User)
            {
                var guildUser = this.Context.Guild.GetUser(this.Context.User.Id);

                if (!guildUser.GuildPermissions.Administrator)
                {
                    await this.Context.Message.AddReactionAsync(Emoji.Parse("❌"));

                    return;
                }
            }

            await _users.Reset(this.Context.User.Id);

            await this.Context.Message.AddReactionAsync(Emoji.Parse("✅"));
        }

        [Command("resetxp all")]
        public async Task Reset()
        {
            var guildUser = this.Context.Guild.GetUser(this.Context.User.Id);

            if (!guildUser.GuildPermissions.Administrator)
            {
                await this.Context.Message.AddReactionAsync(Emoji.Parse("❌"));

                return;
            }

            await _users.ResetAll();

            await this.Context.Message.AddReactionAsync(Emoji.Parse("✅"));
        }

        [Command("profile")]
        public async Task Profile(SocketGuildUser? user = null, RoleMentionPeriod period = RoleMentionPeriod.Any, RoleMentionStatus status = RoleMentionStatus.Any)
        {
            user ??= this.Context.Guild.GetUser(this.Context.User.Id);
            await _paginator.Add(new ProfilePaginator(user, _users.Get(user.Id), _users, period, status, _quickCharts), await this.ReplyAsync("..."));
        }

        [Command("profile")]
        public async Task Profile(ulong user, RoleMentionPeriod period = RoleMentionPeriod.Any, RoleMentionStatus status = RoleMentionStatus.Any)
        {
            await this.Profile(this.Context.Guild.GetUser(user), period, status);
        }
    }
}
