using MediatR;

namespace ApplicationApi.Commands.RejectLoanApplication;

public record RejectLoanApplicationRequest(Guid ApplicationId) : IRequest<Unit>;