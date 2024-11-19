
namespace LoanDecisionApi.LoanApprovalEngine.Rules;

public interface ILoanAcceptanceRule
{
    public bool Evaluate(LoanApplication application);
}