using System.Text.Json;
using JsonException = System.Text.Json.JsonException;

namespace PollySandbox;

public class TimeZoneInfoJsonConverter : System.Text.Json.Serialization.JsonConverter<TimeZoneInfo>
{
    public override TimeZoneInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Accept: null
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        // Example: "Pacific/Honolulu" (string)
        if (reader.TokenType == JsonTokenType.String)
        {
            var id = reader.GetString();
            return TryFindTimeZoneById(id);
        }

        // Example: { "id": "Pacific/Honolulu", ... }
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            if (!doc.RootElement.TryGetProperty("id", out var idProp))
                return null;

            var id = idProp.GetString();
            return TryFindTimeZoneById(id);
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing TimeZoneInfo.");
    }

    private static TimeZoneInfo TryFindTimeZoneById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }
        catch (Exception e) when (e is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            var message = $"[TimeZoneInfoJsonConverter] Unrecognized timezone id '{id}': {e.Message}";
            Console.WriteLine(message);
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, TimeZoneInfo value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }
        
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("displayName", value.DisplayName);
        writer.WriteString("standardName", value.StandardName);
        writer.WriteString("daylightName", value.DaylightName);
        writer.WriteString("baseUtcOffset", value.BaseUtcOffset.ToString());
        writer.WriteBoolean("supportsDaylightSavingTime", value.SupportsDaylightSavingTime);
        writer.WriteBoolean("hasIanaId", value.HasIanaId);

        writer.WriteEndObject();
    }
}