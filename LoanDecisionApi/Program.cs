using Dapr.Client;
using LoanDecisionApi;
using LoanDecisionApi.LoanApprovalEngine;
using LoanDecisionApi.Events.LoanApproved;
using LoanDecisionApi.Events.LoanRejected;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddLoanApplication();
builder.Services.AddDaprClient();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapPost("loanReview", async (LoanApplication loanApplication, LoanApplicationApprovalEngine loanApplicationApprovalEngine, DaprClient daprClient, CancellationToken cancellationToken = default(CancellationToken)) =>
    {
        var approved = await loanApplicationApprovalEngine.Evaluate(loanApplication);
        if (approved)
        {
            await daprClient.PublishEventAsync("pubsub", "loan-approved", LoanApprovedEvent.CreateFromApplication(loanApplication), new Dictionary<string, string> { { "applicationId", loanApplication.ApplicationId.ToString() } }, cancellationToken);
        }
        else
        {
            await daprClient.PublishEventAsync("pubsub", "loan-rejected", LoanRejectedEvent.CreateFromApplication(loanApplication), new Dictionary<string, string> { { "applicationId", loanApplication.ApplicationId.ToString() } }, cancellationToken);
        }
    })
    .WithTopic("pubsub", "loan-approval-request")
;

app.Run();

public partial class Program { }
public record LoanApplication(Guid ApplicationId, decimal Amount, decimal AssetValue, int CreditScore)
{
    public decimal LoanToValuePercentage => Math.Round((Amount / AssetValue) * 100, 2);
}