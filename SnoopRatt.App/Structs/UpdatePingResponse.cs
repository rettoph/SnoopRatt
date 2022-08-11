using SnoopRatt.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Structs
{
    public record UpdatePingResponse(Ping Ping, User User, bool LeveledUp)
    {
    }
}
