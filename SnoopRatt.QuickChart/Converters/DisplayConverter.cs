using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnoopRatt.QuickChart.Converters
{
    internal sealed class DisplayConverter : JsonConverter<Display>
    {
        public override Display Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Display value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case Display.True:
                    writer.WriteBooleanValue(true);
                    break;
                case Display.False:
                    writer.WriteBooleanValue(false);
                    break;
                case Display.Auto:
                    writer.WriteStringValue("auto");
                    break;
            }
        }
    }
}
