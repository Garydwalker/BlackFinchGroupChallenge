﻿using ApplicationDomain.Domain;
using ApplicationDomain.Stores;
using CommunityToolkit.Diagnostics;
using MediatR;

namespace ApplicationApi.Commands;

public class LoanApplicationRequestHandler(LoanApplicationStore loanApplicationStore) : IRequestHandler<LoanApplicationRequest, Unit>
{
    public async Task<Unit> Handle(LoanApplicationRequest request, CancellationToken cancellationToken)
    {
        Guard.IsInRange(request.CreditScore,1,999);

        var loanApplication = LoanApplication.Create(request.LoanAmount, request.AssetValue, request.CreditScore);
        await loanApplicationStore.Create(loanApplication);
        return Unit.Value;
}
}