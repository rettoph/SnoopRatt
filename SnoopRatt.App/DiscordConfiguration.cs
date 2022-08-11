using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App
{
    public class DiscordConfiguration
    {
        public string? AppId { get; set; }
        public string? Key { get; set; }
        public string? ClientId { get; set; }
        public string? Token { get; set; }
        public string? Permissions { get; set; }
        public string[] Prefixes { get; set; }
    }
}
