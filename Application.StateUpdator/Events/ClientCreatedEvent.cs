namespace Application.Events.Realisation;

public class ClientCreatedEvent
{
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
}