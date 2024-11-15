using LoanApplicationApp.Domain;
using MediatR;

namespace LoanApplicationApp.Events.LoanApplicationComplete;

public record LoanApplicationCompleteEvent(LoanApplication Application, DateTime RequestTime) : INotification
{
    public static LoanApplicationCompleteEvent CreateLoanApplication(LoanApplication application) =>
        new(application, DateTime.UtcNow);
}