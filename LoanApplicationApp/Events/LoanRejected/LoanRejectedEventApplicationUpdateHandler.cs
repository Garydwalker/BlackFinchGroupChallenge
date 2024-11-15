using LoanApplicationApp.Stores;
using MediatR;

namespace LoanApplicationApp.Events.LoanRejected;

public class LoanRejectedEventApplicationUpdateHandler(LoanApplicationStore loanApplicationStore) : INotificationHandler<LoanRejectedEvent>
{
    public async Task Handle(LoanRejectedEvent notification, CancellationToken cancellationToken)
    {
        var application = await loanApplicationStore.Get(notification.Application.Id);
        if (application is null)
        {
            throw new ArgumentException($"Could not find Application with id : {notification.Application.Id}");
        }

        application.Reject();

        await loanApplicationStore.Update(application);
    }
}