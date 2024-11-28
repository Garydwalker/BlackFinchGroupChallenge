using BlazorApp1.Components;
using BlazorApp1.Hubs;
using Google.Api;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDaprClient();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); 
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddSingleton<ActiveLoanApplicationsClients>();
var app = builder.Build();
app.UseResponseCompression();
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    Console.WriteLine($"environment: {app.Environment}");
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

}




app.UseAntiforgery();
app.UseCloudEvents();
app.MapSubscribeHandler();
app.MapPost("/loan-application-complete", async (LoanApplicationCompleteEvent eventMessage, ActiveLoanApplicationsClients clients) =>
    {
        await clients.BroadCastMessage(eventMessage);
        return Results.Ok();
    })
    .WithTopic("pubsub", "loan-application-complete")
    .WithName("ApplicationComplete");

app.MapHub<LoanApplicationHub>("/loan-application-hub");


app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();
