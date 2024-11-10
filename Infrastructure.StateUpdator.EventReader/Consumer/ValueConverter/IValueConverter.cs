using Application.Events.Abstractions;
using CSharpFunctionalExtensions;

namespace Infrastructure.StateUpdator.ValueConverter;

public interface IValueConverter<TInput>
{
    public Result<TOutpute> Convert<TOutpute>(TInput input)
        where TOutpute : IDBStateChangeEvent;
}