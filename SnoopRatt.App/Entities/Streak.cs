using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Entities
{
    public class Streak
    {
        public DateTime First { get; set; }
        public DateTime Last { get; set; }
        public Ping[] Pings { get; set; }
    }
}
