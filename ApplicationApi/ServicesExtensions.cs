using ApplicationDomain.LoanApprovalEngine;
using ApplicationDomain.LoanApprovalEngine.Rules;
using ApplicationDomain.Stores;

namespace ApplicationApi;

public static class ServicesExtensions
{
    public static void AddLoanApplication(this IServiceCollection services)
    {
        services.AddScoped<LoanApplicationStore>();
        services.AddSingleton<LoanApplicationApprovalEngine>();
        services.AddSingleton<ILoanAcceptanceRule, AllowedValuesLoanAcceptanceRule>();
        services.AddSingleton<ILoanAcceptanceRule, MillionPoundLoanAcceptanceRule>();
        services.AddSingleton<ILoanAcceptanceRule, SubMillionPoundLoanAcceptanceRule>();
    }
}