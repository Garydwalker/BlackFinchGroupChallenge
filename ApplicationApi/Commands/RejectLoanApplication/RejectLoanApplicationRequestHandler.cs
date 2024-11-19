using ApplicationDomain.Stores;
using MediatR;

namespace ApplicationApi.Commands.RejectLoanApplication;

public class RejectLoanApplicationRequestHandler(ILoanApplicationStore loanApplicationStore) : IRequestHandler<RejectLoanApplicationRequest, Unit>
{

    public async Task<Unit> Handle(RejectLoanApplicationRequest request, CancellationToken cancellationToken)
    {
        var application = await loanApplicationStore.Get(request.ApplicationId);
        if (application is null) throw new Exception("Application not found");

        application.Reject();

        return Unit.Value;
    }
}