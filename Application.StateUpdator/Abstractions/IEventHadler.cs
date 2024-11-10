using Application.Events.Abstractions;
using CSharpFunctionalExtensions;

namespace Application.Handlers.Abstcations;

public interface IEventHadler<T> 
    where T : IDBStateChangeEvent
{
    public Task<Result> HandleAsync(T @event);
}