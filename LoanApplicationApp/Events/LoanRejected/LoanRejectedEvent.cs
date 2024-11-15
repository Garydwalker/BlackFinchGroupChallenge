using LoanApplicationApp.Domain;
using MediatR;

namespace LoanApplicationApp.Events.LoanRejected;

public record LoanRejectedEvent(LoanApplication Application, DateTime RequestDate) : INotification
{
    public static LoanRejectedEvent CreateFromApplication(LoanApplication application) =>
        new(application, DateTime.UtcNow);
}