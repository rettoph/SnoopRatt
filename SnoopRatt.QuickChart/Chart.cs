using System.Text.Json.Serialization;

namespace SnoopRatt.QuickChart
{
    public class Chart
    {
        [JsonPropertyName("type")]
        public ChartType Type { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }

        [JsonPropertyName("options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Options? Options { get; set; }
    }
}