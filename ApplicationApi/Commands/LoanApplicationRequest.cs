using MediatR;

namespace ApplicationApi.Commands;

public record LoanApplicationRequest(decimal LoanAmount, decimal AssetValue, int CreditScore) : IRequest<Unit>;