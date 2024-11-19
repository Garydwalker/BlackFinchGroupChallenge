using Dapr.Client;
using ReportingApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<LoanStatsStore>();
builder.AddServiceDefaults();
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

app.MapGet("/stats", (LoanStatsStore loanStatsStore) => loanStatsStore.GetStats())
.WithName("GetStats");


app.MapPost("stats/events/new-application", async (LoanApplication loanApplication, LoanStatsStore loanStatsStore, DaprClient daprClient, CancellationToken cancellationToken = default(CancellationToken)) =>
    {
       loanStatsStore.Update(loanApplication);
       return Results.Ok();
    })
    .WithTopic("pubsub", "loan-application-complete")
    ;
app.Run();

public class LoanApplication
{
    public Guid ApplicationId { get; init; }
    public decimal Amount { get; init; }
    public decimal AssetValue { get; init; }
    public int CreditScore { get; init; }
    public bool? ApprovalStatus { get; init; }
    public decimal LoanToValuePercentage => Math.Round((Amount / AssetValue) * 100, 2);
}