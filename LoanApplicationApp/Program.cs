using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using LoanApplicationApp;
using LoanApplicationApp.Commands;
using LoanApplicationApp.LoanApprovalEngine;
using LoanApplicationApp.LoanApprovalEngine.Rules;
using LoanApplicationApp.RequestPipeline;
using LoanApplicationApp.Stores;
using MediatR;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<LoanRequestService>();
builder.Services.AddSingleton<LoanApplicationStore>();
builder.Services.AddSingleton<LoanStatsStore>();
builder.Services.AddSingleton<LoanApplicationApprovalEngine>();
builder.Services.AddSingleton<ILoanAcceptanceRule, AllowedValuesLoanAcceptanceRule>();
builder.Services.AddSingleton<ILoanAcceptanceRule, MillionPoundLoanAcceptanceRule>();
builder.Services.AddSingleton<ILoanAcceptanceRule, SubMillionPoundLoanAcceptanceRule>();
builder.Services.AddValidatorsFromAssemblyContaining<LoanApplicationRequestValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddMediatR(cfg=>
{
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.RegisterServicesFromAssemblyContaining<LoanApplicationRequest>();
});


builder.Services.AddHostedService<LoanSystemHostedService>();
var host = builder.Build();


await host.RunAsync();