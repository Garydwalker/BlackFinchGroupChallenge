using LoanDecisionApi.LoanApprovalEngine;
using LoanDecisionApi.LoanApprovalEngine.Rules;

namespace LoanDecisionApi;

public static class ServicesExtensions
{
    public static void AddLoanApplication(this IServiceCollection services)
    {
        services.AddSingleton<LoanApplicationApprovalEngine>();
        services.AddSingleton<ILoanAcceptanceRule, AllowedValuesLoanAcceptanceRule>();
        services.AddSingleton<ILoanAcceptanceRule, MillionPoundLoanAcceptanceRule>();
        services.AddSingleton<ILoanAcceptanceRule, SubMillionPoundLoanAcceptanceRule>();
    }
}