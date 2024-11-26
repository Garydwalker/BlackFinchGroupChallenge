using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;

namespace BlazorApp1.Hubs
{
    public class BaseEvent
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }
        public DateTime RequestTime { get; init; }
    }

    public class LoanApplicationCompleteEvent : BaseEvent
    {
        public Guid ApplicationId { get; init; }
        public bool? ApprovalStatus { get; init; }

    }

    public class ActiveLoanApplicationsClients(IHubContext<LoanApplicationHub> hubContext)
    {
        public Dictionary<Guid, string> ActiveLoanApplicationClients = new();

        public async Task BroadCastMessage(LoanApplicationCompleteEvent eventMessage)
        {
            if (ActiveLoanApplicationClients.ContainsKey(eventMessage.ApplicationId) is false) return;

            await hubContext.Clients.Client(ActiveLoanApplicationClients[eventMessage.ApplicationId]).SendAsync("LoanApplicationUpdated", eventMessage.ApprovalStatus);
        }
    }
    public class LoanApplicationHub(ActiveLoanApplicationsClients activeClients) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var applicationId = Context.GetHttpContext().Request.Query["applicationId"].ToString();
            activeClients.ActiveLoanApplicationClients.Add(Guid.Parse(applicationId), Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var applicationId = Context.GetHttpContext().Request.Query["applicationId"].ToString();
            activeClients.ActiveLoanApplicationClients.Remove(Guid.Parse(applicationId));
            await base.OnDisconnectedAsync(exception);
        }
    }
}
