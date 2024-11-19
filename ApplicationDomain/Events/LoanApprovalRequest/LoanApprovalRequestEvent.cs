using ApplicationDomain.Domain;

namespace ApplicationDomain.Events.LoanApprovalRequest;

public class LoanApprovalRequestEvent : BaseEvent
{
    public Guid ApplicationId { get; init; }
    public decimal Amount { get; init; }
    public decimal AssetValue { get; init; }
    public int CreditScore { get; init; }

    public static LoanApprovalRequestEvent CreateLoanApplication(LoanApplication application) =>
        new()
        {
            Id = Guid.NewGuid(),
            ApplicationId= application.Id,
            Amount = application.Amount,
            AssetValue = application.AssetValue,
            CreditScore = application.CreditScore
        };
}