using Application.Events.Abstractions;
using Common;
using CSharpFunctionalExtensions;

namespace Infrastructure.StateUpdator.ValueConverter;

public class JsonValueConverter<TEve> : IValueConverter<string>
{
    public Result<TOutpute> Convert<TOutpute>(string input) where TOutpute : IDBStateChangeEvent
    {
        return ResultJsonDeserialiser.Deserialise<TOutpute>(input);
    }
}