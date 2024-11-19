using MediatR;

namespace LoanApplicationApp.Domain;

public class BaseEvent 
{
    protected BaseEvent()
    {
        Id = Guid.NewGuid();
        RequestTime = DateTime.UtcNow;
    }
    public Guid Id { get; init; }
    private DateTime RequestTime { get; init; }
}
public abstract class DomainEntity
{
    private readonly List<BaseEvent> _events = [];
    public Guid Id { get; init; }
    public IReadOnlyList<BaseEvent> Events => _events;
    public void AddEvent(BaseEvent newEvent)
    {
        _events.Add(newEvent);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}