using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SnoopRatt.App.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Services
{
    public class DiscordCommandService : IHostedService
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private ILogger<DiscordCommandService> _logger;
        private IServiceProvider _provider;
        private DiscordConfiguration _config;

        public DiscordCommandService(
            ILogger<DiscordCommandService> logger, 
            DiscordSocketClient client, 
            CommandService commands, 
            IServiceProvider provider, 
            DiscordConfiguration config)
        {
            _logger = logger;
            _client = client;
            _commands = commands;
            _provider = provider;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _commands.CommandExecuted += this.CommandExecutedAsync;
            _client.MessageReceived += this.MessageReceivedAsync;
            _commands.Log += this.LogAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message))
                return;
            if (message.Source != MessageSource.User)
                return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            // (!message.HasCharPrefix('!', ref argPos))
            // for a more traditional command format like !help.

            if(message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                // Perform the execution of the command. In this method,
                // the command service will perform precondition and parsing check
                // then execute the command if one is matched.
                var scope = _provider.CreateScope();
                await _commands.ExecuteAsync(context, argPos, scope.ServiceProvider);
                scope.Dispose();
                // Note that normally a result will be returned by this format, but here
                // we will handle the result in CommandExecutedAsync,

                return;
            }

            foreach (string prefix in _config.Prefixes)
            {
                if(message.HasStringPrefix(prefix, ref argPos))
                {
                    var context = new SocketCommandContext(_client, message);
                    // Perform the execution of the command. In this method,
                    // the command service will perform precondition and parsing check
                    // then execute the command if one is matched.
                    var scope = _provider.CreateScope();
                    await _commands.ExecuteAsync(context, argPos, scope.ServiceProvider);
                    scope.Dispose();
                    // Note that normally a result will be returned by this format, but here
                    // we will handle the result in CommandExecutedAsync,

                    return;
                }
            }
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
        }

        private Task LogAsync(LogMessage arg)
        {
            _logger.Log(arg.Severity.ToLogLevel(), arg.Message);

            return Task.CompletedTask;
        }
    }
}
