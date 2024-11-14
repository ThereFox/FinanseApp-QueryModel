using Application.Events.Abstractions;

namespace Application.Events.Realisation;

public class ClientCreatedEvent : IDBStateChangeEvent
{
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
}