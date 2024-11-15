using LoanApplicationApp.Domain;
using MediatR;

namespace LoanApplicationApp.Events.LoanApprovalRequest;

public record LoanApprovalRequestEvent(LoanApplication Application, DateTime RequestTime) : INotification
{
    public static LoanApprovalRequestEvent CreateLoanApplication(LoanApplication application) =>
        new(application, DateTime.UtcNow);
}