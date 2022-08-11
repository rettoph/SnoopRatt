using Image = SixLabors.ImageSharp.Image;
using Color = SixLabors.ImageSharp.Color;
using Font = SixLabors.Fonts.Font;

using Discord;
using Discord.WebSocket;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Extensions;
using SnoopRatt.App.Entities;
using SnoopRatt.App.Services;
using SnoopRatt.QuickChart;
using SnoopRatt.QuickChart.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace SnoopRatt.App.Utilities
{
    public class ProfilePaginator : Paginator
    {
        private int _rank;
        private readonly QuickChartService _quickCharts;
        private readonly SocketGuildUser _socketUser;
        private readonly UserService _users;

        public const int PingsPerPage = 9;
        public const int StaticPages = 2;

        public override int Max => StaticPages + (int)Math.Ceiling((this.Filtered().Count() / (float)PingsPerPage));

        public User User { get; }

        public RoleMentionPeriod Period { get; }
        public RoleMentionStatus Status { get; }

        public ProfilePaginator(SocketGuildUser socketUser, User user, UserService users, RoleMentionPeriod period, RoleMentionStatus status, QuickChartService quickCharts)
        {
            _socketUser = socketUser;
            _quickCharts = quickCharts;
            _users = users;
            
            this.Period = period;
            this.Status = status;
            this.User = user;

            _rank = _users.GetRank(this.User.Id);
        }

        public override async Task Load(int page, EmbedBuilder embed, List<FileAttachment> attachments)
        {
            if(page == 0)
            {
                await this.OverviewPage(RoleMentionPeriod.Evening, "☀️ Evening Overview", this.User.AverageEveningTimestamp?.Ticks, embed, attachments);
                return;
            }

            if (page == 1)
            {
                await this.OverviewPage(RoleMentionPeriod.Morning, "🌙 Morning Overview", this.User.AverageMorningTimestamp?.Ticks, embed, attachments);
                return;
            }

            await this.PingsPage(page - StaticPages, embed);
        }

        private void Header(EmbedBuilder embed)
        {
            embed.AddField("User", _socketUser.Mention, true);
            embed.AddField("Period", this.Period, true);
            embed.AddField("Status", this.Status, true);
        }

        private async Task OverviewPage(RoleMentionPeriod period, string title, long? averageTimestampTicks, EmbedBuilder embed, List<FileAttachment> attachments)
        {
            var pings = this.User.Pings.Where(x => x.Period == period);
            var values = pings.GroupBy(x => x.Status).ToDictionary(x => x.Key, x => x.Count());
            var streaks = pings.GetStreaks();
            
            var latestStreak = streaks?.MaxBy(x => x.Pings[0].TimeStamp);
            var currentStreak = latestStreak is null || (DateTime.Now - latestStreak.Pings.Last().TimeStamp).Days >= 1 ? null : latestStreak;
            var bestStreak = streaks?.MaxBy(x => x.Pings.Length);

            embed.WithTitle(title);

            embed.AddField("Current Streak", $"[{currentStreak?.Pings.Length ?? 0}]({currentStreak?.Pings[0].Url})", true);
            embed.AddField("Best Streak", $"[{bestStreak?.Pings.Length ?? 0}]({bestStreak?.Pings[0].Url})", true);
            embed.AddField("Total", pings.Count().ToString("#,##0"), true);
            
            if(values.Count() > 0)
            {
                embed.AddField("Status", string.Join('\n', values.Select(v => v.Key)), true);
                embed.AddField("Count", string.Join('\n', values.Select(v => v.Value.ToString("#,##0"))), true);
                embed.AddField("Average Time", averageTimestampTicks is null ? "--:--:--.---" : new DateTime(averageTimestampTicks.Value).ToString(@"h\:mm\:ss.fff"), true);
            }

            using (var image = new Image<Rgba32>(650, 563))
            {
                await User.AddHeader(image, this.User, _socketUser, _rank);

                using (var chart = await _quickCharts.GetSecondsLineChart(pings))
                {
                    image.Mutate(x =>
                    {
                        x.FillPolygon(new Color(new Rgba32(255, 255, 255, 225)), new PointF(25, 178), new PointF(625, 178), new PointF(625, 538), new PointF(25, 538));
                        x.DrawImage(chart, new Point(25, 178), 1);
                    });
                }

                var page = await image.GetPngAttachmentAsync();
                attachments.Add(page);
                embed.WithImageUrl($"attachment://{page.FileName}");
            }
        }

        private Task PingsPage(int page, EmbedBuilder embed)
        {
            var items = this.Filtered().Skip(page * PingsPerPage).Take(PingsPerPage);

            string statuses = string.Join('\n', items.Select(x => x.Status.ToString()));
            string periods = string.Join('\n', items.Select(x => x.Period.ToString()));
            string times = string.Join('\n', items.Select(x => $"[{x.TimeStamp.ToString("MM-dd-yyyy hh:mm:ss tt")}]({x.Url})"));

            embed.AddField("Time", times, true);
            embed.AddField("Period", periods, true);
            embed.AddField("Status", statuses, true);

            return Task.CompletedTask;
        }

        private void AddValue<TKey>(TKey key, string label, Dictionary<TKey, int> values, EmbedBuilder embed, bool inline)
            where TKey : notnull
        {
            if (values.TryGetValue(key, out int value))
            {
                embed.AddField(label, value, inline);
            }
        }

        private IEnumerable<Ping> Filtered()
        {
            return this.User.Pings.Where(x => this.Period.HasFlag(x.Period) && this.Status.HasFlag(x.Status));
        }
    }
}
