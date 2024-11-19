
using ApplicationDomain.Domain;

namespace ApplicationDomain.LoanApprovalEngine.Rules;

public interface ILoanAcceptanceRule
{
    public bool Evaluate(LoanApplication application);
}