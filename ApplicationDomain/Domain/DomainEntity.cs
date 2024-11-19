using ApplicationDomain.Events.LoanApplicationComplete;
using ApplicationDomain.Events.LoanApprovalRequest;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicationDomain.Domain;
public class DiscriminatorFirstConverter<T> : JsonConverter<T>
{

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Use default deserialization
        return JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var jsonObject = JsonSerializer.SerializeToElement(value, options).EnumerateObject();
        writer.WriteStartObject();

        // Write the discriminator property first
        foreach (var property in jsonObject)
        {
            if (property.Name.Equals("$type", StringComparison.OrdinalIgnoreCase))
            {
                property.WriteTo(writer);
                break;
            }
        }

        // Write the rest of the properties
        foreach (var property in jsonObject)
        {
            if (!property.Name.Equals("$type", StringComparison.OrdinalIgnoreCase))
            {
                property.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }
}
[JsonConverter(typeof(DiscriminatorFirstConverter<BaseEvent>))]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type", IgnoreUnrecognizedTypeDiscriminators = true, UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(LoanApplicationCompleteEvent), typeDiscriminator: "LoanApplicationCompleteEvent")]
[JsonDerivedType(typeof(LoanApprovalRequestEvent), typeDiscriminator: "LoanApprovalRequestEvent")]
public class BaseEvent
{
    protected BaseEvent()
    {
        Id = Guid.NewGuid();
        RequestTime = DateTime.UtcNow;
    }
    public Guid Id { get; init; }
    public DateTime RequestTime { get; init; }

}
public abstract class DomainEntity
{
    private readonly List<BaseEvent> _events = [];
    public Guid Id { get; init; }
    public IReadOnlyList<BaseEvent> Events => _events;
    public void AddEvent(BaseEvent newEvent)
    {
        _events.Add(newEvent);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}