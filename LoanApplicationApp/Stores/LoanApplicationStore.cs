using LoanApplicationApp.Domain;
using MediatR;

namespace LoanApplicationApp.Stores;

public class LoanApplicationStore(IMediator mediator)
{
    private readonly List<LoanApplication> _applications = [];

    public virtual Task<LoanApplication?> Get(Guid applicationId)
    {
        return  Task.FromResult(_applications.FirstOrDefault(x => x.Id == applicationId));
    }
    
    public virtual async Task Create(LoanApplication application)
    {
        _applications.Add(application);
        await PublishEvents(application);
    }

    public virtual async Task Update(LoanApplication application)
    {
        var existingApplication = _applications.Find(x => x.Id == application.Id);
        if(existingApplication is null)
            throw new InvalidOperationException("Application not found");
        
        _applications.Remove(existingApplication);

        _applications.Add(application);
        await PublishEvents(application);
    }

    private async Task PublishEvents(LoanApplication application)
    {
        var eventsToPublish = application.Events.ToList();
        application.ClearEvents();
        foreach (var notification in eventsToPublish)
        {
            await mediator.Publish(notification);
        }
    }
}