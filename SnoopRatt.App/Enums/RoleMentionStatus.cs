using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Enums
{
    [Flags]
    public enum RoleMentionStatus
    {
        None = 1,
        Early = 2,
        OnTime = 4,
        Late = 8,
        Any = None | Early | OnTime | Late,
        OnTimeOrLate = OnTime | Late
    }
}
