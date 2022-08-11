using SnoopRatt.App;
using SnoopRatt.App.Services;
using SnoopRatt.App.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord.Commands;
using SnoopRatt.App.Database;
using SnoopRatt.App.Utilities;
using SnoopRatt.QuickChart.Services;
using Discord;

Host.CreateDefaultBuilder()
    .ConfigureLogging(logger =>
    {
        logger.AddConsole();

#if DEBUG
        logger.SetMinimumLevel(LogLevel.Trace);
#else
        logger.SetMinimumLevel(LogLevel.Information);
#endif
    })
    .ConfigureServices(services =>
    {
        var configuration = BuildConfiguration();

        services.AddSingleton(configuration.Bind<DiscordConfiguration>("Discord"))
            .AddSingleton(configuration.Bind<DabaseConfiguration>("Database"))
            .AddSingleton(configuration.Bind<PaginatorConfiguration>("Paginators"))
            .AddSingleton<DiscordSocketClient>(p => new DiscordSocketClient(new DiscordSocketConfig()
            {
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers
            }))
            .AddSingleton<CommandService>()
            .AddSingleton<PaginatorService>()
            .AddScoped<GuildService>()
            .AddScoped<GuildSettingsService>()
            .AddScoped<UserService>()
            .AddScoped<PingService>()
            .AddScoped<QuickChartService>()
            .AddTransient<TimeZonePaginator>();

        services.AddHostedService<DiscordService>()
            .AddHostedService<DiscordCommandService>()
            .AddHostedService<PaginatorRefreshService>();

        services.AddDbContext<MaryJaneContext>();
    }).Build().Run();


IConfiguration BuildConfiguration()
{
    var configuration = new ConfigurationBuilder();

#if DEBUG
    configuration.AddJsonFile("Config.development.json");
#else
    configuration.AddJsonFile("Config.json");
#endif

    return configuration.Build();
}