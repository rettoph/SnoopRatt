using SnoopRatt.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Entities
{
    public class Ping
    {
        public static readonly TimeSpan EveningTimeSpan = new TimeSpan(16, 20, 0);
        public static readonly TimeSpan MorningTimeSpan = new TimeSpan(4, 20, 0);

        public ulong MessageId { get; set; }
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime TimeStamp { get; set; }
        public RoleMentionStatus Status { get; set; }
        public RoleMentionPeriod Period { get; set; }
        public double Xp { get; set; }
        public int Streak { get; set; }

        public string Url => $"https://discord.com/channels/{this.ChannelId}/{this.GuildId}/{this.MessageId}";

        public static RoleMentionStatus GetStatus(DateTime timestamp)
        {
            var eveningOffset = timestamp.TimeOfDay.Subtract(EveningTimeSpan);
            var morningOffset = timestamp.TimeOfDay.Subtract(MorningTimeSpan);

            if (eveningOffset.TotalMinutes >= 0 && eveningOffset.TotalMinutes <= 1)
            {
                return RoleMentionStatus.OnTime;
            }
            if (morningOffset.TotalMinutes >= 0 && morningOffset.TotalMinutes <= 1)
            {
                return RoleMentionStatus.OnTime;
            }

            var closest = Math.Abs(eveningOffset.TotalMinutes) < Math.Abs(morningOffset.TotalMinutes) ? eveningOffset : morningOffset;

            if (closest.TotalMinutes > 0 && closest.TotalMinutes < 2)
            {
                return RoleMentionStatus.Late;
            }

            if (closest.TotalMinutes < 0 && closest.TotalMinutes > -2)
            {
                return RoleMentionStatus.Early;
            }

            return RoleMentionStatus.None;
        }

        public static RoleMentionPeriod GetPeriod(DateTime timestamp)
        {
            var eveningOffset = timestamp.TimeOfDay.Subtract(EveningTimeSpan);
            var morningOffset = timestamp.TimeOfDay.Subtract(MorningTimeSpan);

            if (Math.Abs(eveningOffset.TotalMinutes) < Math.Abs(morningOffset.TotalMinutes))
            {
                return RoleMentionPeriod.Evening;
            }

            return RoleMentionPeriod.Morning;
        }
    }
}
