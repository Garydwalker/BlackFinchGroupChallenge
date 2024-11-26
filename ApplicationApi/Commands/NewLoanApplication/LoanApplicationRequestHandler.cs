using ApplicationDomain.Domain;
using ApplicationDomain.Stores;
using CommunityToolkit.Diagnostics;
using MediatR;

namespace ApplicationApi.Commands.NewLoanApplication;

public class LoanApplicationRequestHandler(ILoanApplicationStore loanApplicationStore) : IRequestHandler<LoanApplicationRequest, Unit>
{
    public async Task<Unit> Handle(LoanApplicationRequest request, CancellationToken cancellationToken)
    {
        Guard.IsInRange(request.CreditScore, 1, 1000);

        var loanApplication = LoanApplication.Create(request.LoanAmount, request.AssetValue, request.CreditScore,request.Id);
        await loanApplicationStore.Create(loanApplication);
        return Unit.Value;
    }
}