using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Enums
{
    [Flags]
    public enum RoleMentionPeriod
    {
        Morning = 1,
        Evening = 2,
        Any = Morning | Evening
    }
}
