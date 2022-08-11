using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SnoopRatt.App.Database;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Entities;
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
    // public class RoleModule : ModuleBase<SocketCommandContext>
    // {
    //     private RoleMentionService _mentions;
    //     private GuildSettingsService _settings;
    //     private PaginatorService _paginator;
    //     private QuickChartService _quickCharts;
    // 
    //     public RoleModule(QuickChartService quickCharts, PaginatorService paginator, RoleMentionService mentions, GuildSettingsService settings)
    //     {
    //         _mentions = mentions;
    //         _settings = settings;
    //         _paginator = paginator;
    //         _quickCharts = quickCharts;
    //     }
    // 
    //     [Command("profile")]
    //     public async Task Profile(SocketUser? user = null, RoleMentionPeriod period = RoleMentionPeriod.Any, RoleMentionStatus status = RoleMentionStatus.Any)
    //     {
    //         _mentions.RefreshAll();
    // 
    //         var mentions = _mentions.Where(x => x.UserId == user.Id)
    //             .OrderByDescending(x => x.TimeStamp)
    //             .ToArray();
    // 
    //         await _paginator.Add(new ProfilePaginator(user ?? this.Context.User, period, status, _quickCharts, mentions), await this.ReplyAsync("..."));
    //     }
    // 
    //     [Command("profile")]
    //     public async Task Profile(ulong user, RoleMentionPeriod period = RoleMentionPeriod.Any, RoleMentionStatus status = RoleMentionStatus.Any)
    //     {
    //         await this.Profile(this.Context.Guild.GetUser(user), period, status);
    //     }
    // 
    //     [Command("mentions backtrack")]
    //     public async Task Backtrack(SocketTextChannel channel, ulong? from = null, int total = 10000)
    //     {
    //         var settings = _settings.Get(this.Context.Guild.Id);
    // 
    //         if (settings.Role is null)
    //         {
    //             return;
    //         }
    // 
    //         var response = await this.ReplyAsync("Searching...");
    //         var start = DateTime.UtcNow;
    // 
    //         var pages = from is null ? channel.GetMessagesAsync(total) : channel.GetMessagesAsync(from.Value, Direction.Before, total);
    //         var messages = 0;
    //         var found = 0;
    //         var count = 0;
    //         IMessage? lastMessage = null;
    // 
    //         await foreach (var page in pages)
    //         {
    //             foreach (var message in page)
    //             {
    //                 messages++;
    //                 count++;
    // 
    //                 if (message.MentionedRoleIds.Contains(settings.Role.Value))
    //                 {
    //                     var timestamp = TimeZoneInfo.ConvertTime(message.Timestamp.UtcDateTime, settings.TimeZone);
    //                     await _mentions.Set(message.Id, this.Context.Guild.Id, message.Channel.Id, message.Author.Id, message.Timestamp.UtcDateTime);
    // 
    //                     found++;
    //                 }
    // 
    //                 if (lastMessage is null || message.Timestamp < lastMessage.Timestamp)
    //                 {
    //                     lastMessage = message;
    //                 }
    //             }
    // 
    //             await response.ModifyAsync(mp =>
    //             {
    //                 mp.Content = $"Found {found} mentions in {messages} messages.\nElapsed time: {DateTime.UtcNow - start}\nLast Message Timestamp: {TimeZoneInfo.ConvertTime(lastMessage?.Timestamp ?? DateTime.MinValue, settings.TimeZone)}\nLast Message Id: {lastMessage?.Id}";
    //             });
    //         }
    // 
    //         await response.ModifyAsync(mp =>
    //         {
    //             mp.Content = $"Done. Found {found} mentions in {messages} messages.\nElapsed time: {DateTime.UtcNow - start}\nLast Message Timestamp: {TimeZoneInfo.ConvertTime(lastMessage?.Timestamp ?? DateTime.MinValue, settings.TimeZone)}\nLast Message Id: {lastMessage?.Id}";
    //         });
    //     }
    // }
}
