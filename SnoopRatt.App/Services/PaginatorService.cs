using Discord;
using Discord.WebSocket;
using SnoopRatt.App.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    public class PaginatorService
    {
        private IServiceProvider _provider;
        private DiscordSocketClient _client;
        private Dictionary<ulong, Paginator> _paginators;
        private ILogger _logger;
        private PaginatorConfiguration _config;

        public PaginatorService(ILogger<PaginatorService> logger, DiscordSocketClient client, IServiceProvider provider, PaginatorConfiguration config)
        {
            _client = client;
            _provider = provider;
            _logger = logger;
            _config = config;

            _paginators = new Dictionary<ulong, Paginator>();

            _client.ReactionAdded += this.HandleReaction;
            _client.ReactionRemoved += this.HandleReaction;
        }

        public async Task<TPaginator> Add<TPaginator>(TPaginator paginator, IUserMessage message)
            where TPaginator : Paginator
        {
            await paginator.Initialize(message);

            _paginators.Add(message.Id, paginator);

            return paginator;
        }

        public async Task<TPaginator> Create<TPaginator>(IUserMessage message)
            where TPaginator : Paginator
        {
            var paginator = _provider.GetRequiredService<TPaginator>();

            return await this.Add(paginator, message);
        }

        public async Task Refresh()
        {
            foreach(Paginator paginator in _paginators.Values)
            {
                var idleTime = (DateTime.UtcNow - paginator.LastAction);
                _logger.LogDebug($"{paginator.Message!.Id} - Idle for {idleTime.ToString()}");

                if (idleTime > TimeSpan.FromMilliseconds(_config.IdleRate))
                {
                    _logger.LogInformation($"marking paginator {paginator.Message!.Id} idle...");

                    await paginator.Idle();

                    _paginators.Remove(paginator.Message!.Id);
                }
            }
        }

        private async Task HandleReaction(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            try
            {
                if (reaction.User.Value.IsBot)
                {
                    return;
                }

                if (_paginators.TryGetValue(message.Id, out var paginator))
                {
                    await paginator.React(reaction.Emote);
                }

            }
            catch(Exception e)
            {

            }
        }
    }
}
