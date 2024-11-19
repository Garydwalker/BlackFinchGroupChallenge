

using ApplicationDomain.Domain;

namespace ApplicationDomain.Events.LoanApplicationComplete;

public class LoanApplicationCompleteEvent : BaseEvent
{
    public Guid ApplicationId { get; init; }
    public decimal Amount { get; init; }
    public decimal AssetValue { get; init; }
    public int CreditScore { get; init; }
    public bool? ApprovalStatus { get; private set; }


    public static LoanApplicationCompleteEvent CreateLoanApplication(LoanApplication application) =>
        new()
        {
            Id = Guid.NewGuid(),
            ApplicationId = application.Id,
            Amount = application.Amount,
            AssetValue = application.AssetValue,
            CreditScore = application.CreditScore,
            ApprovalStatus= application.ApprovalStatus
        };
}