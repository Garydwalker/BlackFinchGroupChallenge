using LoanApplicationApp.Domain;
using MediatR;

namespace LoanApplicationApp.Events.LoanApproved;

public record LoanApprovedEvent(LoanApplication Application, DateTime RequestDate) : INotification
{
    public static LoanApprovedEvent CreateFromApplication(LoanApplication application) =>
        new(application, DateTime.UtcNow);
}