using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using LoanApplicationApp;
using LoanApplicationApp.Commands;
using LoanApplicationApp.RequestPipeline;
using MediatR;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLoanApplication();

builder.Services.AddValidatorsFromAssemblyContaining<LoanApplicationRequestValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddMediatR(cfg =>
{
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.RegisterServicesFromAssemblyContaining<LoanApplicationRequest>();
});


builder.Services.AddHostedService<LoanSystemHostedService>();
var host = builder.Build();


await host.RunAsync();