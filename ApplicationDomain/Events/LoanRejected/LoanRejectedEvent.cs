using ApplicationDomain.Domain;

namespace ApplicationDomain.Events.LoanRejected;

public class LoanRejectedEvent : BaseEvent
{
    public LoanApplication Application { get; init; }

    private LoanRejectedEvent(LoanApplication application)
    {
        Application = application;
    }
    public static LoanRejectedEvent CreateFromApplication(LoanApplication application) =>
        new(application);
}