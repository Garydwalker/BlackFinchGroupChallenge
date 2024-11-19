using MediatR;

namespace ApplicationApi.Commands.NewLoanApplication;

public record LoanApplicationRequest(decimal LoanAmount, decimal AssetValue, int CreditScore) : IRequest<Unit>;