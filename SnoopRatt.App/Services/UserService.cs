using SnoopRatt.App.Database;
using SnoopRatt.App.Entities;
using SnoopRatt.App.Enums;
using SnoopRatt.App.Extensions;
using SnoopRatt.App.Structs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    public class UserService
    {
        private MaryJaneContext _context;
        private GuildSettingsService _settings;

        public UserService(MaryJaneContext context, GuildSettingsService settings)
        {
            _context = context;
            _settings = settings;
        }

        public async Task Delete(ulong id)
        {
            var user = this.Get(id);

            user.Xp = 0;

            _context.RemoveRange(user.Pings);

            await _context.SaveChangesAsync();
        }

        public async Task ResetAll()
        {
            foreach (ulong id in _context.Users.Select(x => x.Id))
            {
                await this.Reset(id);
            }
        }

        public async Task Reset(ulong id)
        {
            var user = this.Get(id);

            user.Xp = 0;

            await _context.SaveChangesAsync();
        }

        public async Task SetAlertOnLevelUp(ulong id, bool value)
        {
            var user = this.Get(id);

            user.AlertOnLevelUp = value;

            await _context.SaveChangesAsync();
        }

        public async Task<UpdatePingResponse> UpdatePing(ulong userId, ulong messageId, ulong guildId, ulong channelId, DateTime timestamp)
        {
            var settings = _settings.Get(guildId);
            var user = this.Get(userId);
            var ping = user.Pings.FirstOrDefault(x => x.MessageId == messageId);

            if (ping is null)
            {
                ping = _context.Pings.CreateProxy();

                ping.MessageId = messageId;
                ping.UserId = userId;
                ping.GuildId = guildId;
                ping.ChannelId = channelId;

                user.Pings.Add(ping);
                _context.Pings.Add(ping);
            }

            ping.TimeStamp = TimeZoneInfo.ConvertTimeFromUtc(timestamp, settings.TimeZone);
            ping.Status = Ping.GetStatus(ping.TimeStamp);
            ping.Period = Ping.GetPeriod(ping.TimeStamp);

            var streaks = user.Pings.GetStreaks();
            var streak = streaks.FirstOrDefault(x => x.Pings.Contains(ping));
            var index = Array.IndexOf(streak?.Pings ?? Array.Empty<Ping>(), ping);

            ping.Streak = index;
            ping.Xp = this.GetXp(ping);

            var oldLevel = user.Level;
            user.Xp += ping.Xp;
            var leveledUp = oldLevel != user.Level && user.AlertOnLevelUp;

            await _context.SaveChangesAsync();

            return new UpdatePingResponse(ping, user, leveledUp);
        }

        public int GetRank(ulong id)
        {
            var rank = 0;

            foreach(User user in _context.Users.OrderByDescending(x => x.Xp))
            {
                if(user.Id == id)
                {
                    return rank;
                }

                rank++;
            }

            return -1;
        }

        public IEnumerable<User> Where(Func<User, bool> predicate)
        {
            return _context.Users.Where(predicate);
        }

        public IEnumerable<User> All(bool eager)
        {
            if(eager)
            {
                return _context.Users.Include(x => x.Pings);
            }

            return _context.Users;
        }

        public User Get(ulong id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == id);

                if (user is null)
                {
                    user = _context.Users.CreateProxy();
                    user.Id = id;

                    _context.Users.Add(user);
                }

                return user;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private double GetXp(Ping ping)
        {
            if(ping.Status != RoleMentionStatus.OnTime)
            {
                return 0d;
            }

            var xp = 1d;
            var modifier = 1d;

            if (ping.Period == RoleMentionPeriod.Morning)
            {
                modifier += 1;
            }

            if(ping.TimeStamp.Second < 1)
            {
                modifier += 0.25d;
            }

            if (ping.TimeStamp.Second < 5)
            {
                modifier += 0.15d;
            }

            if (ping.TimeStamp.Second < 10)
            {
                modifier += 0.05d;
            }

            if (ping.Streak != -1)
            {
                modifier += (ping.Streak * 0.2d);
            }

            return xp *= modifier;
        }
    }
}
