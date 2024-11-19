using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;



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
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type",IgnoreUnrecognizedTypeDiscriminators = true,UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(LoanApplicationCompleteEvent), typeDiscriminator: "LoanApplicationCompleteEvent")]
[JsonDerivedType(typeof(LoanApprovalRequestEvent), typeDiscriminator: "LoanApprovalRequestEvent")]
public class BaseEvent
{
    public Guid Id { get; init; }
    public DateTime RequestTime { get; init; }
}

public class LoanApplicationCompleteEvent : BaseEvent
{
    public Guid ApplicationId { get; init; }
    public decimal Amount { get; init; }
    public decimal AssetValue { get; init; }
    public int CreditScore { get; init; }
    public bool? ApprovalStatus { get; private set; }

}
public class LoanApprovalRequestEvent : BaseEvent
{
    public Guid ApplicationId { get; init; }
    public decimal Amount { get; init; }
    public decimal AssetValue { get; init; }
    public int CreditScore { get; init; }

}

namespace ChangeFeedFunctions
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
           
        }

        [Function(nameof(Function1))]
        public async Task Run(
            [CosmosDBTrigger(
                databaseName: "applications",
                containerName: "LoanApplications",
                Connection = "Cosmos",
                LeaseContainerName = "leases",
                CreateLeaseContainerIfNotExists = true,
                StartFromBeginning = true)]
            IReadOnlyList<object> input)
        {
            
            foreach (var baseEvent in input)
            {
                try
                {
                    var serialize = JsonSerializer.Serialize(baseEvent);
                    var newEvent = JsonSerializer.Deserialize<BaseEvent>(serialize);
                    _logger.LogInformation("Processed event: {event}", newEvent);
                }
                catch(Exception ex)
                {
                 continue;
                }
                
                Console.WriteLine("hello");
            }
        }
    }
}
