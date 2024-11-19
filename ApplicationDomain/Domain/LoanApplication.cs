
using ApplicationDomain.Events.LoanApplicationComplete;
using ApplicationDomain.Events.LoanApprovalRequest;

namespace ApplicationDomain.Domain;

public class LoanApplication: DomainEntity
{
    
    public decimal Amount { get; init; }
    public decimal AssetValue { get; init; }
    public int CreditScore { get; init; }
    public bool? ApprovalStatus { get; private set; }
    public decimal LoanToValuePercentage => Math.Round((Amount / AssetValue) * 100, 2);

    private LoanApplication(Guid id, decimal amount, decimal assetValue, int creditScore)
    {
        Id = id;
        Amount = amount;
        AssetValue = assetValue;
        CreditScore = creditScore;
    }

    public static LoanApplication Create(decimal amount, decimal assetValue, int creditScore, Guid? id = null)
    {
        var loanApplication = new LoanApplication(id?? Guid.NewGuid(), amount, assetValue, creditScore);
        loanApplication.AddEvent(LoanApprovalRequestEvent.CreateLoanApplication(loanApplication));
        return loanApplication;
    }

    public void Approve()
    {
        ApprovalStatus = true;
        AddEvent(LoanApplicationCompleteEvent.CreateLoanApplication(this));
    }

    public void Reject()
    {
        ApprovalStatus = false;
        AddEvent(LoanApplicationCompleteEvent.CreateLoanApplication(this));
    }
}