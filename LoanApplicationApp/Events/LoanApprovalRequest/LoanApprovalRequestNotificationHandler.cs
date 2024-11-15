using LoanApplicationApp.Events.LoanApproved;
using LoanApplicationApp.Events.LoanRejected;
using LoanApplicationApp.LoanApprovalEngine;
using MediatR;

namespace LoanApplicationApp.Events.LoanApprovalRequest;

public class LoanApprovalRequestNotificationHandler(LoanApplicationApprovalEngine loanApplicationApprovalEngine, IMediator mediator) : INotificationHandler<LoanApprovalRequestEvent>
{
    public async Task Handle(LoanApprovalRequestEvent notification, CancellationToken cancellationToken)
    {
        var loanApprovalStatus = await loanApplicationApprovalEngine.Evaluate(notification.Application);

        if (loanApprovalStatus)
            await mediator.Publish(LoanApprovedEvent.CreateFromApplication(notification.Application), cancellationToken);
        else
            await mediator.Publish(LoanRejectedEvent.CreateFromApplication(notification.Application), cancellationToken);

    }
}