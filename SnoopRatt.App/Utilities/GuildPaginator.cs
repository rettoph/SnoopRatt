using Discord;
using Discord.WebSocket;
using SnoopRatt.App.Entities;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Utilities
{
    public class GuildPaginator : Paginator
    {
        private SocketGuild _guild;

        public List<User> Users { get; }
        public List<Ping> Pings { get; }

        public GuildPaginator(SocketGuild guild, List<User> users)
        {
            _guild = guild;
            this.Users = users;
            this.Pings = this.Users.SelectMany(x => x.Pings).ToList();
        }

        public override int Max => 13;

        public override Task Load(int page, EmbedBuilder embed, List<FileAttachment> attachments)
        {
            if (page == 0)
            {
                return this.OrderedValuesPage(
                    "Levels & XP", 
                    this.Users.OrderByDescending(x => x.Xp).ThenByDescending(x => x.Pings.Count(x => x.Status == RoleMentionStatus.OnTime)),
                    "Level",
                    x => $"{x.Level}".ToString(), 
                    embed, 
                    attachments,
                    "XP",
                    x => x.Xp.ToString("#,###,##0.##"));
            }

            if (page == 1)
            {
                return this.OrderedValuesPage(
                    "On Time Evening Pings", 
                    this.Users.OrderByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Evening && x.Status == RoleMentionStatus.OnTime)), 
                    "Total",
                    x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Evening && x.Status == RoleMentionStatus.OnTime).ToString("#,###"), 
                    embed, 
                    attachments);
            }

            if (page == 2)
            {
                return this.OrderedValuesPage(
                    "Evening Streaks - Current",
                    this.Users.OrderByDescending(x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Evening).GetStreaks();
                        var latestStreak = streaks?.MaxBy(x => x.Pings[0].TimeStamp);
                        var currentStreak = latestStreak is null || (DateTime.Now - latestStreak.Pings.Last().TimeStamp).Days >= 1 ? null : latestStreak;

                        return currentStreak?.Pings.Length ?? 0;
                    }).ThenByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Evening && x.Status == RoleMentionStatus.OnTime)),
                    "Streak",
                    x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Evening).GetStreaks();
                        var latestStreak = streaks?.MaxBy(x => x.Pings[0].TimeStamp);
                        var currentStreak = latestStreak is null || (DateTime.Now - latestStreak.Pings.Last().TimeStamp).Days >= 1 ? null : latestStreak;

                        return (currentStreak?.Pings.Length ?? 0).ToString();
                    },
                    embed,
                    attachments);
            }

            if (page == 3)
            {
                return this.OrderedValuesPage(
                    "Evening Streaks - Best of All Time",
                    this.Users.OrderByDescending(x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Evening).GetStreaks();
                        var bestStreak = streaks.Count() > 0 ? streaks.Max(x => x.Pings.Length) : 0;

                        return bestStreak;
                    }).ThenByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Evening && x.Status == RoleMentionStatus.OnTime)),
                    "Streak",
                    x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Evening).GetStreaks();
                        var bestStreak = streaks.Count() > 0 ? streaks.Max(x => x.Pings.Length) : 0;

                        return bestStreak.ToString();
                    },
                    embed,
                    attachments);
            }

            if (page == 4)
            {
                return this.OrderedValuesPage(
                    "Fastest Evening Pings - 30 Day Average",
                    this.Users.Where(x => x.AverageEveningTimestamp is not null).OrderBy(x => x.AverageEveningTimestamp).ThenByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Evening && x.Status == RoleMentionStatus.OnTime)),
                    "Time",
                    x => new DateTime(x.AverageEveningTimestamp!.Value.Ticks).ToString("h:mm:ss.fff"),
                    embed,
                    attachments);
            }

            if (page == 5)
            {
                return this.OrderedPingsdPage(
                    "Fastest Evening Pings",
                    this.Pings.Where(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Evening).OrderBy(x => x.TimeStamp.TimeOfDay),
                    embed,
                    attachments);
            }

            if (page == 6)
            {
                return this.OrderedPingsdPage(
                    "Slowest Evening Pings",
                    this.Pings.Where(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Evening).OrderByDescending(x => x.TimeStamp.TimeOfDay),
                    embed,
                    attachments);
            }

            if (page == 7)
            {
                return this.OrderedValuesPage(
                    "On Time Morning Pings",
                    this.Users.OrderByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Morning && x.Status == RoleMentionStatus.OnTime)),
                    "Total",
                    x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Morning && x.Status == RoleMentionStatus.OnTime).ToString("#,###"),
                    embed,
                    attachments);
            }

            if (page == 8)
            {
                return this.OrderedValuesPage(
                    "Morning Streaks - Current",
                    this.Users.OrderByDescending(x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Morning).GetStreaks();
                        var latestStreak = streaks?.MaxBy(x => x.Pings[0].TimeStamp);
                        var currentStreak = latestStreak is null || (DateTime.Now - latestStreak.Pings.Last().TimeStamp).Days >= 1 ? null : latestStreak;

                        return currentStreak?.Pings.Length ?? 0;
                    }).ThenByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Morning && x.Status == RoleMentionStatus.OnTime)),
                    "Streak",
                    x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Morning).GetStreaks();
                        var latestStreak = streaks?.MaxBy(x => x.Pings[0].TimeStamp);
                        var currentStreak = latestStreak is null || (DateTime.Now - latestStreak.Pings.Last().TimeStamp).Days >= 1 ? null : latestStreak;

                        return (currentStreak?.Pings.Length ?? 0).ToString();
                    },
                    embed,
                    attachments);
            }

            if (page == 9)
            {
                return this.OrderedValuesPage(
                    "Morning Streaks - Best of All Time",
                    this.Users.OrderByDescending(x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Morning).GetStreaks();
                        var bestStreak = streaks.Count() > 0 ? streaks.Max(x => x.Pings.Length) : 0;

                        return bestStreak;
                    }).ThenByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Morning && x.Status == RoleMentionStatus.OnTime)),
                    "Streak",
                    x =>
                    {
                        var streaks = x.Pings.Where(x => x.Period == RoleMentionPeriod.Morning).GetStreaks();
                        var bestStreak = streaks.Count() > 0 ? streaks.Max(x => x.Pings.Length) : 0;

                        return bestStreak.ToString();
                    },
                    embed,
                    attachments);
            }

            if (page == 10)
            {
                return this.OrderedValuesPage(
                    "Fastest Morning Pings - 30 Day Average",
                    this.Users.Where(x => x.AverageMorningTimestamp is not null).OrderBy(x => x.AverageMorningTimestamp).ThenByDescending(x => x.Pings.Count(x => x.Period == RoleMentionPeriod.Morning && x.Status == RoleMentionStatus.OnTime)),
                    "Time",
                    x => new DateTime(x.AverageMorningTimestamp!.Value.Ticks).ToString("h:mm:ss.fff"),
                    embed,
                    attachments);
            }

            if (page == 11)
            {
                return this.OrderedPingsdPage(
                    "Fastest Morning Pings",
                    this.Pings.Where(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Morning).OrderBy(x => x.TimeStamp.TimeOfDay),
                    embed,
                    attachments);
            }

            if (page == 12)
            {
                return this.OrderedPingsdPage(
                    "Slowest Morning Pings",
                    this.Pings.Where(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Morning).OrderByDescending(x => x.TimeStamp.TimeOfDay),
                    embed,
                    attachments);
            }

            throw new ArgumentOutOfRangeException(nameof(page));
        }

        private Task OrderedPingsdPage(string title, IEnumerable<Ping> pings, EmbedBuilder embed, List<FileAttachment> attachments)
        {
            embed.WithTitle(title);

            var items = pings.Take(9);

            string users = string.Join('\n', items.Select(x => $"<@{x.UserId}>"));
            string dates = string.Join('\n', items.Select(x => $"{x.TimeStamp.ToString("MM-dd-yyyy")}"));
            string times = string.Join('\n', items.Select(x => $"[{x.TimeStamp.ToString("h:mm:ss.fff tt")}]({x.Url})"));


            embed.AddField("Time", times, true);
            embed.AddField("Date", dates, true);
            embed.AddField("User", users, true);

            return Task.CompletedTask;
        }

        private Task OrderedValuesPage(string title, IEnumerable<User> ordered, string valueTitle, Func<User, string> valueSelector, EmbedBuilder embed, List<FileAttachment> attachments, string value2Title = null, Func<User, string> value2Selector = null)
        {
            embed.WithTitle(title);

            string users = string.Empty;
            string values = string.Empty;
            string values2 = string.Empty;

            var rank = 1;
            var count = 0;

            foreach(User user in ordered)
            {
                var socketUser = _guild.GetUser(user.Id);

                if(socketUser is null)
                {
                    continue;
                }

                users += rank switch
                {
                    1 => "🥇",
                    2 => "🥈",
                    3 => "🥉",
                    _ => " " + rank.ToString()
                } + ". ";

                users += $"{socketUser.Mention}\n";
                values += valueSelector(user) + "\n";

                if(value2Selector is not null)
                {
                    values2 += value2Selector(user) + "\n";
                }

                rank++;

                if(++count >= 20)
                {
                    break;
                }
            }

            embed.AddField("User", users.TrimEnd('\n'), true);
            embed.AddField(valueTitle, values.TrimEnd('\n'), true);
            if (value2Selector is not null)
            {
                embed.AddField(value2Title, values2.TrimEnd('\n'), true);
            }

            return Task.CompletedTask;
        }
    }
}
