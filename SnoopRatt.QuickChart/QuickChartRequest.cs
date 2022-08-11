using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnoopRatt.QuickChart
{
    public class QuickChartRequest
    {
        [JsonPropertyName("backgroundColor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BackgroundColor { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; } = 500;

        [JsonPropertyName("height")]
        public int Height { get; set; } = 300;

        [JsonPropertyName("format")]
        public string Format { get; set; } = "png";

        [JsonPropertyName("chart")]
        public Chart Chart { get; set; }
    }
}
