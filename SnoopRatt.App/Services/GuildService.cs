using Discord;
using Discord.WebSocket;
using SnoopRatt.App.Database;
using SnoopRatt.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    public class GuildService
    {
        private MaryJaneContext _context;
        private UserService _users;
        private GuildSettingsService _settings;

        public GuildService(MaryJaneContext context, UserService users, GuildSettingsService settings)
        {
            _context = context;
            _users = users;
            _settings = settings;
        }

        public async Task Refresh(ulong guild, SocketTextChannel channel, ulong from, IUserMessage? reply)
        {
            var messages = channel.GetMessagesAsync(from, Direction.After, int.MaxValue, new RequestOptions()
            {
                RetryMode = Discord.RetryMode.RetryRatelimit
            });

            var total = 0;
            var count = 0;

            var settings = _settings.Get(guild);

            if(settings.Role is null)
            {
                return;
            }

            var pings = new List<IMessage>();

            await foreach(var batch in messages)
            {
                foreach(var message in batch.OrderBy(x => x.Timestamp))
                {
                    if(message.MentionedRoleIds.Contains(settings.Role.Value))
                    {
                        await _users.UpdatePing(message.Author.Id, message.Id, guild, message.Channel.Id, message.Timestamp.UtcDateTime);

                        pings.Add(message);
                    }

                    count++;

                    if(count % 1000 == 0)
                    {
                        if(reply is not null)
                        {
                            await reply.ModifyAsync(mp =>
                            {
                                mp.Content = $"Scanned {count} messages, found {pings.Count()} pings...";
                            });
                        }
                    }
                }
            }

            await reply.ModifyAsync(mp =>
            {
                mp.Content = $"Done scanning... Saving...";
            });

            count = 0;
            foreach (IMessage message in pings.OrderBy(x => x.Timestamp))
            {
                await _users.UpdatePing(message.Author.Id, message.Id, guild, message.Channel.Id, message.Timestamp.UtcDateTime);
                count++;

                if (count % 1000 == 0)
                {
                    if (reply is not null)
                    {
                        await reply.ModifyAsync(mp =>
                        {
                            mp.Content = $"Saved {count} pings...";
                        });
                    }
                }
            }

            await reply.ModifyAsync(mp =>
            {
                mp.Content = $"Done.";
            });
        }
    }
}
