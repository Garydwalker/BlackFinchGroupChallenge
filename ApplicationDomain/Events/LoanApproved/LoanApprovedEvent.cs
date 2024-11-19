using ApplicationDomain.Domain;

namespace ApplicationDomain.Events.LoanApproved;

public class LoanApprovedEvent : BaseEvent
{
    public LoanApplication Application { get; init; }

    private LoanApprovedEvent(LoanApplication application)
    {
        Application = application;
    }
    public static LoanApprovedEvent CreateFromApplication(LoanApplication application) =>
        new(application);
}