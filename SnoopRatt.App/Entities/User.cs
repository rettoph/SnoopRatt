using Discord.WebSocket;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Extensions;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Entities
{
    public class User
    {
        public static Image BackgroundImage = Image.Load("Content/header.png");
        public static Pen BackgroundPen = new Pen(new Color(new Rgba32(0, 0, 0, 150)), 128);
        public static Font HeaderFont;
        public static Font SubHeaderFont;
        public static Pen XpPen = new Pen(new Color(new Rgba32(128,30,30, 255)), 4);

        private static HttpClient _client = new HttpClient();

        private int _rank;

        static User()
        {
            var fonts = new FontCollection();
            fonts.AddSystemFonts();

            HeaderFont = fonts.Get("Arial").CreateFont(70);
            SubHeaderFont = fonts.Get("Segoe UI Emoji").CreateFont(22);
        }

        public ulong Id { get; set; }

        public double Xp { get; set; }

        public bool AlertOnLevelUp { get; set; } = false;

        public int Level => User.CalculateLevel(this.Xp);

        public virtual ICollection<Ping> Pings { get; set; }
        public TimeSpan? AverageEveningTimestamp => this.Pings.Count(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Evening && x.TimeStamp > DateTime.Now.AddDays(-60)) < 1 ? null : new TimeSpan(Convert.ToInt64(this.Pings.Where(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Evening && x.TimeStamp > DateTime.Now.AddDays(-60)).Average(x => x.TimeStamp.TimeOfDay.Ticks)));
        public TimeSpan? AverageMorningTimestamp => this.Pings.Count(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Morning && x.TimeStamp > DateTime.Now.AddDays(-60)) < 1 ? null : new TimeSpan(Convert.ToInt64(this.Pings.Where(x => x.Status == RoleMentionStatus.OnTime && x.Period == RoleMentionPeriod.Morning && x.TimeStamp > DateTime.Now.AddDays(-60)).Average(x => x.TimeStamp.TimeOfDay.Ticks)));

        public static async Task AddHeader(Image image, User user, SocketGuildUser socketUser, int rank)
        {
            try
            {
                using (var avatarStream = await _client.GetStreamAsync(socketUser.GetAvatarUrl(Discord.ImageFormat.Png)))
                {
                    using (var avatar = Image.Load(avatarStream))
                    {
                        image.Mutate(x =>
                        {
                            x.DrawImage(BackgroundImage, 1f);
                            x.DrawLines(BackgroundPen, new PointF(25, 89), new PointF(650, 89));

                            x.DrawImage(avatar, new Point(25, 25), 1);

                            var font = HeaderFont.ScaleToWidth(socketUser.DisplayName, 447);
                            x.DrawText(socketUser.DisplayName, font, Color.White, new PointF(178, ((80 - font.Size) / 2) + 25));

                            string rankText = $"Rank: #{rank + 1}" + rank switch
                            {
                                0 => " 🥇",
                                1 => " 🥈",
                                2 => " 🥉",
                                _ => ""
                            };

                            x.DrawText($"Level: {user.Level}   |   {user.Xp.ToString("#,###,##0.#")} XP   |   {rankText}", SubHeaderFont, Color.White, new PointF(178, ((28 - SubHeaderFont.Size) / 2) + 110));

                            var currentLevelXp = User.CalculateXp(user.Level);
                            var nextLevelXp = User.CalculateXp(user.Level + 1);

                            var ratio = (user.Xp - currentLevelXp) / (nextLevelXp - currentLevelXp);

                            x.DrawLines(XpPen, new PointF(153, 151), new PointF(153 + (float)(447f * ratio), 151));
                        });
                    }
                }
            }
            catch (Exception e)
            {

            }

        }

        public static double CalculateXp(int level)
        {
            return Math.Pow(level / 100d, 2) / 0.0002d;
        }

        public static int CalculateLevel(double xp)
        {
            return (int)(100d * Math.Sqrt(0.0002d * xp));
        }
    }
}
