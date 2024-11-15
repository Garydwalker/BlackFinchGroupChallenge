using LoanApplicationApp.Stores;
using MediatR;

namespace LoanApplicationApp.Events.LoanApproved;

public class LoanApprovedEventApplicationUpdateHandler(LoanApplicationStore loanApplicationStore) : INotificationHandler<LoanApprovedEvent>
{
    public async Task Handle(LoanApprovedEvent notification, CancellationToken cancellationToken)
    {
        var application = await loanApplicationStore.Get(notification.Application.Id);
        if (application is null)
        {
            throw new ArgumentException($"Could not find Application with id : {notification.Application.Id}");
        }

        application.Approve();

        await loanApplicationStore.Update(application);
    }
}