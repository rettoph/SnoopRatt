using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnoopRatt.QuickChart.Converters
{
    internal sealed class ChartTypeConverter : JsonConverter<ChartType>
    {
        public override ChartType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ChartType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}
