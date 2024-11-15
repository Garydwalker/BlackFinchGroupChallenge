using MediatR;

namespace LoanApplicationApp.Events.LoanRejected;

public class LoanRejectedEventConsoleWriterHandler : INotificationHandler<LoanRejectedEvent>
{
    public Task Handle(LoanRejectedEvent notification, CancellationToken cancellationToken)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Sorry application:{notification.Application.Id} can not be Approved");
        Console.ForegroundColor = ConsoleColor.White;

        return Task.CompletedTask;
    }
}