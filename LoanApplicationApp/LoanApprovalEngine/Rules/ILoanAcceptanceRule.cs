using LoanApplicationApp.Domain;

namespace LoanApplicationApp.LoanApprovalEngine.Rules;

public interface ILoanAcceptanceRule
{
    public bool Evaluate(LoanApplication application);
}