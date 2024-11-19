

using ApplicationDomain.Domain;

namespace ApplicationDomain.LoanApprovalEngine.Rules;

public class AllowedValuesLoanAcceptanceRule :ILoanAcceptanceRule
{
    public bool Evaluate(LoanApplication application) => application.Amount is >= 100000 and <= 1500000;
}