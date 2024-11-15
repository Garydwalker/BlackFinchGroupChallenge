using Microsoft.Extensions.Hosting;

namespace LoanApplicationApp;

public sealed class LoanSystemHostedService(LoanRequestService loanRequestService) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return loanRequestService.StartAsync();
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}