using CommunityToolkit.Diagnostics;
using LoanApplicationApp.Domain;
using LoanApplicationApp.Stores;
using MediatR;

namespace LoanApplicationApp.Commands;

public class LoanApplicationRequestHandler(LoanApplicationStore loanApplicationStore) : IRequestHandler<LoanApplicationRequest, Unit>
{
    public async Task<Unit> Handle(LoanApplicationRequest request, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(request.LoanAmount);
        Guard.IsNotNull(request.AssetValue);
        Guard.IsNotNull(request.CreditScore);
        Guard.IsInRange(request.CreditScore.Value, 1, 1000);

        await loanApplicationStore.Create(LoanApplication.Create(request.LoanAmount.Value,
            request.AssetValue.Value, request.CreditScore.Value));
        return Unit.Value;
    }
}