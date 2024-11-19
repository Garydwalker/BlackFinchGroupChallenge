using LoanDecisionApi.Events.LoanApproved;

namespace LoanDecisionApi.Events.LoanRejected;

public record LoanRejectedEvent(Guid ApplicationId, DateTime RequestDate)
{
    public static LoanRejectedEvent CreateFromApplication(LoanApplication application) =>
        new(application.ApplicationId, DateTime.UtcNow);
}