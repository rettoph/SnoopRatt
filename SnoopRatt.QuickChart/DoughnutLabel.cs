using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnoopRatt.QuickChart
{
    public class DoughnutLabel
    {
        [JsonPropertyName("labels")]
        public IEnumerable<Label> Labels { get; set; }
    }
}
