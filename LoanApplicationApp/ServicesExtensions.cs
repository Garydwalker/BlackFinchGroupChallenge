using LoanApplicationApp.LoanApprovalEngine;
using LoanApplicationApp.LoanApprovalEngine.Rules;
using LoanApplicationApp.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApplicationApp;

public static class ServicesExtensions
{
    public static void AddLoanApplication(this IServiceCollection services)
    {
        services.AddTransient<LoanRequestService>();
        services.AddSingleton<LoanApplicationStore>();
        services.AddSingleton<LoanStatsStore>();
        services.AddSingleton<LoanApplicationApprovalEngine>();
        services.AddSingleton<ILoanAcceptanceRule, AllowedValuesLoanAcceptanceRule>();
        services.AddSingleton<ILoanAcceptanceRule, MillionPoundLoanAcceptanceRule>();
        services.AddSingleton<ILoanAcceptanceRule, SubMillionPoundLoanAcceptanceRule>();
    }
}