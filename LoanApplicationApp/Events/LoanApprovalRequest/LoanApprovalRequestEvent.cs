using LoanApplicationApp.Domain;
using MediatR;

namespace LoanApplicationApp.Events.LoanApprovalRequest;

public class LoanApprovalRequestEvent : BaseEvent
{
    public LoanApplication Application { get; init; }

    private LoanApprovalRequestEvent(LoanApplication application)
    {
        Application = application;
    }

    public static LoanApprovalRequestEvent CreateLoanApplication(LoanApplication application) =>
        new(application);
}