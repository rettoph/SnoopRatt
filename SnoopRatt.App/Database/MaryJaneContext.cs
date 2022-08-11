using SnoopRatt.App.Database.Configuration;
using SnoopRatt.App.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Database
{
    public class MaryJaneContext : DbContext
    {
        private DabaseConfiguration _config;

        public DbSet<GuildSettings> GuildSettings { get; set; }
        public DbSet<Ping> Pings { get; set; }
        public DbSet<User> Users { get; set; }

        public MaryJaneContext(DabaseConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseLazyLoadingProxies().UseSqlite($"Data Source={_config.Path}");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration<GuildSettings>(new GuildSettingsConfiguration());
            builder.ApplyConfiguration<Ping>(new RoleMentionsConfiguration());
            builder.ApplyConfiguration<User>(new UsersConfiguration());
        }
    }
}
