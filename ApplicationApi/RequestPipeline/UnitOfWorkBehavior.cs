using ApplicationDomain.Stores;
using MediatR;

namespace ApplicationApi.RequestPipeline;

public class UnitOfWorkBehavior<TRequest, TResponse>(ApplicationDbContext context)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result =await next();

            
        await context.SaveChangesAsync(cancellationToken);

        return result;
    }
}