using MediatR;

namespace LoanApplicationApp.Domain;

public abstract class DomainEntity
{
    private readonly List<INotification> _events = [];
    public Guid Id { get; init; }
    public IReadOnlyList<INotification> Events => _events;
    public void AddEvent(INotification newEvent)
    {
        _events.Add(newEvent);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}