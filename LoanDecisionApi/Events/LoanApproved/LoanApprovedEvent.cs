namespace LoanDecisionApi.Events.LoanApproved;

public record LoanApprovedEvent(Guid ApplicationId, DateTime RequestDate) 
{
    public static LoanApprovedEvent CreateFromApplication(LoanApplication loanApplication) =>
        new(loanApplication.ApplicationId, DateTime.UtcNow);
}