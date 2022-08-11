using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnoopRatt.QuickChart
{
    public class Data
    {
        [JsonPropertyName("labels")]
        public IEnumerable<object> Labels { get; set; }

        [JsonPropertyName("datasets")]
        public IEnumerable<DataSet> DataSets { get; set; }
    }
}
