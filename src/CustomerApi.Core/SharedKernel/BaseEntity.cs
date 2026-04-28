

namespace CustomerApi.Core.SharedKernel;
public abstract class BaseEntity : IEntity<Guid>
{
    private readonly List<BaseEvent> _doamainEvents = [];

    protected BaseEntity() => Id = Guid.NewGuid();
    protected BaseEntity(Guid id) => Id = id;   

    public IEnumerable<BaseEvent> DomainEvents =>
        _doamainEvents.AsReadOnly();
    public Guid Id { get; protected set; }

    public void AddDomainEvents(BaseEvent domainEvent) =>
        _doamainEvents.Add(domainEvent);

    public  void ClearDomainEvents() =>
        _doamainEvents.Clear();
}

