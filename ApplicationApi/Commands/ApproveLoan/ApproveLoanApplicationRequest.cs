using MediatR;

namespace ApplicationApi.Commands.ApproveLoan;

public record ApproveLoanApplicationRequest(Guid ApplicationId) : IRequest<Unit>;