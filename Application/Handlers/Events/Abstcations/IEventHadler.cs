using Application.Events.Abstractions;

namespace Application.Handlers.Abstcations;

public interface IEventHadler<T> 
    where T : IEvent
{
    public void Handle(T @event);
}