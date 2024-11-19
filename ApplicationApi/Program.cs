using ApplicationApi;
using ApplicationApi.Commands.ApproveLoan;
using ApplicationApi.Commands.NewLoanApplication;
using ApplicationApi.Commands.RejectLoanApplication;
using ApplicationApi.RequestPipeline;
using ApplicationDomain.Stores;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddLoanApplication();
builder.Services.AddDaprClient();
builder.Services.AddMediatR(cfg =>
{
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.RegisterServicesFromAssemblyContaining<LoanApplicationRequest>();
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddCosmosDbContext<ApplicationDbContext>("cosmos", "applications");

var app = builder.Build();


app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCloudEvents();
app.MapSubscribeHandler();


app.MapPost("/application", async (LoanApplicationRequest  loanApplication, IMediator mediator) =>
{
    if(loanApplication is null)return Results.BadRequest();
    await mediator.Send(loanApplication);

    return Results.Ok();
})
.WithTopic("pubsub", "createApplications")
.WithName("NewApplication");


app.MapPost("/application/events/approve", async (ApproveLoanApplicationRequest request, IMediator mediator) =>
{

    await mediator.Send(request);

    return Results.Ok();
}).WithTopic("pubsub", "loan-approved")
    .WithName("ApplicationApproved");

app.MapPost("/application/events/reject", async (RejectLoanApplicationRequest request, IMediator mediator) =>
    {
        await mediator.Send(request);

        return Results.Ok();
    })
    .WithTopic("pubsub", "loan-rejected")
    .WithName("ApplicationRejected");



app.Run();

namespace ApplicationApi
{
    public partial class Program { }
}
