using LoanApplicationApp.Stores;
using MediatR;

namespace LoanApplicationApp.Events.LoanApplicationComplete;

public class LoanApplicationCompleteEventHandler(LoanStatsStore loanStatsStore) : INotificationHandler<LoanApplicationCompleteEvent>
{
    public Task Handle(LoanApplicationCompleteEvent notification, CancellationToken cancellationToken)
    {
        loanStatsStore.Update(notification.Application);
        return Task.CompletedTask;
    }
}