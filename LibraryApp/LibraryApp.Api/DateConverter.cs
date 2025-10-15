using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibraryApp.Api
{

    public class DateConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString() ?? string.Empty);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("MM/dd/yyyy"));
        }
    }
}
