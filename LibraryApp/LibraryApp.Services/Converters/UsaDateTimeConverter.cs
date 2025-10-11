using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibraryApp.Application.Converters
{
    public class UsaDateTimeConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "MM/dd/yyyy"; // Define custom format

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialization handle (JSON a C#)
            if (DateTime.TryParseExact(reader.GetString(), DateFormat,
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None,
                                       out var date))
            {
                return date;
            }
            throw new JsonException($"The value '{reader.GetString()}' is not a valid Date format.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // This handles the deserialization (C# to JSON)
            // Converts DateTime object to the desired format string
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}
