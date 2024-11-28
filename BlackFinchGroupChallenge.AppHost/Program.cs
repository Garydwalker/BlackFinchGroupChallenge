using Aspire.Hosting;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddConnectionString("cosmos");
    // var cosmos =  builder.AddAzureCosmosDB("cosmos");
    // var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var daprPubSub = builder.AddDaprPubSub("pubsub");

builder.AddProject<Projects.BlazorApp1>("blazorapp1")
    .WithDaprSidecar()
    .WithReference(daprPubSub);

builder.AddProject<Projects.ApplicationApi>("applicationapi")
    .WithDaprSidecar()
    .WithReference(daprPubSub)
    .WithReference(cosmos);

builder.AddAzureFunctionsProject<Projects.ChangeFeedFunctions>("changefeedfunctions")
    // .WithHostStorage(storage)
    .WithDaprSidecar()
    .WithReference(daprPubSub)
    .WithReference(cosmos);


builder.AddProject<Projects.LoanDecisionApi>("loandecisionapi")
    .WithDaprSidecar()
    .WithReference(daprPubSub);


builder.AddProject<Projects.ReportingApi>("reportingapi")
    .WithDaprSidecar()
    .WithReference(daprPubSub);


builder.Build().Run();