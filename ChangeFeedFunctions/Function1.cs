using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dapr.Client;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class BaseEvent
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    public DateTime RequestTime { get; init; }
}

public class LoanApplicationCompleteEvent : BaseEvent
{
    public Guid ApplicationId { get; init; }
    public decimal Amount { get; init; }
    public decimal AssetValue { get; init; }
    public int CreditScore { get; init; }
    public bool? ApprovalStatus { get; init; }

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
    public static class StringExtensions
    {
        public static string ToKebabCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(char.ToLower(input[0]));
            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    stringBuilder.Append('-');

                }
                stringBuilder.Append(char.ToLower(input[i]));
            }

            return stringBuilder.ToString();
        }
    }
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly DaprClient _daprClient;
        private readonly Dictionary<string, Type> _knownEvents;

        public Function1(ILogger<Function1> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
            _knownEvents = new Dictionary<string, Type>
            {
                {nameof(LoanApplicationCompleteEvent), typeof(LoanApplicationCompleteEvent)},
                {nameof(LoanApprovalRequestEvent), typeof(LoanApprovalRequestEvent)}
            };
        }

        [Function(nameof(Function1))]

        public async Task Run(
            [CosmosDBTrigger(
                databaseName: "applications",
                containerName: "LoanApplications",
                Connection = "ConnectionStrings:Cosmos",
                LeaseContainerName = "leases",
                CreateLeaseContainerIfNotExists = true,
                StartFromBeginning = true)]
            IReadOnlyList<dynamic> input)
        {

            foreach (var baseEvent in input)
            {
                if (!baseEvent.TryGetProperty("Discriminator", out JsonElement typeElement)) continue;

                var eventType = typeElement.GetString();
                if (eventType is null || !_knownEvents.ContainsKey(eventType)) continue;

                string jsonString = baseEvent.GetRawText();
                var eventTypeObject = JsonSerializer.Deserialize(jsonString, _knownEvents[eventType]);
                _logger.LogInformation("Processed event: {event}", eventTypeObject);

                await _daprClient.PublishEventAsync("pubsub", eventType.Replace("Event", "").ToKebabCase(), eventTypeObject);
            }
        }
    }
}
