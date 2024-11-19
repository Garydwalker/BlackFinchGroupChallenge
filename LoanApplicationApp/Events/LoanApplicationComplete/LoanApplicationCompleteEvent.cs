using LoanApplicationApp.Domain;
using MediatR;

namespace LoanApplicationApp.Events.LoanApplicationComplete;

public class LoanApplicationCompleteEvent : BaseEvent
{
    public LoanApplication Application { get; init; }

    private LoanApplicationCompleteEvent(LoanApplication application)
    {
        Application = application;
    }
    public static LoanApplicationCompleteEvent CreateLoanApplication(LoanApplication application) =>
        new(application);
}