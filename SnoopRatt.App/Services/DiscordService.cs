using Discord;
using Discord.WebSocket;
using SnoopRatt.App.Entities;
using SnoopRatt.App.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    public class DiscordService : IHostedService
    {
        private const string InviteUrl = "https://discord.com/api/oauth2/authorize?client_id={0}&permissions={1}&scope=bot%20applications.commands";

        private ILogger<DiscordService> _logger;
        private DiscordSocketClient _client;
        private DiscordConfiguration _config;
        private IServiceProvider _provider;

        public DiscordService(IServiceProvider provider, ILogger<DiscordService> logger, DiscordSocketClient client, DiscordConfiguration config)
        {
            _logger = logger;
            _client = client;
            _config = config;
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Invite Link: " + string.Format(DiscordService.InviteUrl, _config.ClientId, _config.Permissions));

            _client.Log += this.LogAsync;
            _client.MessageReceived += this.HandleMessageRecieved;

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private Task LogAsync(LogMessage arg)
        {
            _logger.Log(arg.Severity.ToLogLevel(), arg.Message);

            return Task.CompletedTask;
        }

        private async Task HandleMessageRecieved(SocketMessage arg)
        {
            if(arg.Channel is not SocketTextChannel channel)
            {
                return;
            }
            else if(arg.MentionedRoles.Count() == 0)
            {
                return;
            }
            else
            {
                using(var scope = _provider.CreateScope())
                {
                    var settings = scope.ServiceProvider.GetRequiredService<GuildSettingsService>().Get(channel.Guild.Id);

                    if(arg.MentionedRoles.FirstOrDefault(x => x.Id == settings.Role) is not null)
                    {
                        var users = scope.ServiceProvider.GetRequiredService<UserService>();

                        var result = await users.UpdatePing(
                            arg.Author.Id,
                            arg.Id,
                            channel.Guild.Id,
                            channel.Id,
                            arg.Timestamp.UtcDateTime);



                        if(result.LeveledUp)
                        {
                            var rank = users.GetRank(result.User.Id);
                            using (var image = new Image<Rgba32>(650, 178))
                            {
                                await User.AddHeader(image, result.User, (SocketGuildUser)arg.Author, rank);

                                var page = await image.GetPngAttachmentAsync();

                                await channel.SendFileAsync(
                                    attachment: page, 
                                    text: $"{arg.Author.Mention} just reached level {result.User.Level}", 
                                    messageReference: new MessageReference(arg.Id));
                            }
                        }
                    }
                }
            }
        }
    }
}
