using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnoopRatt.QuickChart
{
    public class Plugins
    {
        [JsonPropertyName("legend")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Legend { get; set; }

        [JsonPropertyName("datalabels")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DataLabels? DataLabels { get; set; }

        [JsonPropertyName("doughnutlabel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DoughnutLabel? DoughnutLabel { get; set; }

        [JsonPropertyName("outlabels")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OutLabels? OutLabels { get; set; }
    }
}
