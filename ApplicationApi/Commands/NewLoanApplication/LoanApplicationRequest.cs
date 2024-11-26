using MediatR;

namespace ApplicationApi.Commands.NewLoanApplication;

public record LoanApplicationRequest(Guid Id,decimal LoanAmount, decimal AssetValue, int CreditScore) : IRequest<Unit>;