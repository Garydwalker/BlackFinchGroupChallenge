using ApplicationDomain.Stores;

namespace ApplicationApi;

public static class ServicesExtensions
{
    public static void AddLoanApplication(this IServiceCollection services)
    {
        services.AddScoped<ILoanApplicationStore,LoanApplicationStore>();
    }
}