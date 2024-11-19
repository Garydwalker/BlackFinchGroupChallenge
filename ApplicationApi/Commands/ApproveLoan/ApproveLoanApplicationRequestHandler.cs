using ApplicationDomain.Stores;
using MediatR;

namespace ApplicationApi.Commands.ApproveLoan;

public class ApproveLoanApplicationRequestHandler(ILoanApplicationStore loanApplicationStore) : IRequestHandler<ApproveLoanApplicationRequest, Unit>
{
    public async Task<Unit> Handle(ApproveLoanApplicationRequest request, CancellationToken cancellationToken)
    {
        var application = await  loanApplicationStore.Get(request.ApplicationId);
        if (application is null) throw new Exception("Application not found");

        application.Approve();
        
        return Unit.Value;
    }
}