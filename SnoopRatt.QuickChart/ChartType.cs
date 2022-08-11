using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.QuickChart
{
    public class ChartType
    {
        public static ChartType Line { get; } = new ChartType("line");
        public static ChartType Pie { get; } = new ChartType("pie");
        public static ChartType Bar { get; } = new ChartType("bar");
        public static ChartType Doughnut { get; } = new ChartType("doughnut");
        public static ChartType OutLabeledPie { get; } = new ChartType("outlabeledPie");

        public readonly string Name;

        internal ChartType(string name)
        {
            this.Name = name;
        }
    }
}
