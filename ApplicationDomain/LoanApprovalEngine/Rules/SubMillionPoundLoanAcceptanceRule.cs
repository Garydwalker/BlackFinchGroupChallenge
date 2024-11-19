using ApplicationDomain.Domain;

namespace ApplicationDomain.LoanApprovalEngine.Rules;

public class SubMillionPoundLoanAcceptanceRule : ILoanAcceptanceRule
{
    public bool Evaluate(LoanApplication application)
    {
        if (application.Amount > 1000000) return true;

        return application.LoanToValuePercentage switch
        {
            < 60 => application.CreditScore >= 750,
            < 80 => application.CreditScore >= 800,
            < 90 => application.CreditScore >= 900,
            >= 90 => false
        };
    }
}