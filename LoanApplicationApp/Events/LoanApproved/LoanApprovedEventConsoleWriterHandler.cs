using MediatR;

namespace LoanApplicationApp.Events.LoanApproved;

public class LoanApprovedEventConsoleWriterHandler : INotificationHandler<LoanApprovedEvent>
{
    public Task Handle(LoanApprovedEvent notification, CancellationToken cancellationToken)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Your application:{notification.Application.Id} has been Approved");
        Console.ForegroundColor = ConsoleColor.White;

        return Task.CompletedTask;
    }
}