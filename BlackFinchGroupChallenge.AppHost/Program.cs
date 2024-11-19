using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddConnectionString("cosmos");

var daprPubSub = builder.AddDaprPubSub("pubsub");

builder.AddProject<Projects.BlazorApp1>("blazorapp1")
    .WithDaprSidecar()
    .WithReference(daprPubSub);

builder.AddProject<Projects.ApplicationApi>("applicationapi")
    .WithDaprSidecar()
    .WithReference(daprPubSub)
    .WithReference(cosmos);

builder.AddAzureFunctionsProject<Projects.ChangeFeedFunctions>("changefeedfunctions")
    .WithReference(cosmos);


builder.Build().Run();
