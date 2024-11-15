using LoanApplicationApp.Domain;

namespace LoanApplicationApp.LoanApprovalEngine.Rules;

public class MillionPoundLoanAcceptanceRule : ILoanAcceptanceRule
{
    public bool Evaluate(LoanApplication application)
    {
        if (application.Amount  < 1000000) return true;
        return application is { LoanToValuePercentage: <= 60, CreditScore: >= 950 };
    }
}