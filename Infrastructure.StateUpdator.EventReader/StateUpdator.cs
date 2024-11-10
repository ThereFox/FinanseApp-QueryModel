using Application.Events.Abstractions;
using Application.Handlers.Abstcations;
using CSharpFunctionalExtensions;
using Infrastructure.StateUpdator.Transaction;
using Infrastructure.StateUpdator.ValueConverter;

namespace Infrastructure.StateUpdator;

public class StateUpdator<TEvent>
    where TEvent : IDBStateChangeEvent
{
    private readonly IEventHadler<TEvent> _handler;
    private readonly IKafkaTransaction<string> _transaction;
    private readonly IValueConverter<string> _converter;
    
    private readonly string _topicName;
    
    public StateUpdator(
        string topicName,
        IKafkaTransaction<string> transaction,
        IEventHadler<TEvent> handler,
        IValueConverter<string> converter)
    {
        _topicName = topicName;
        _transaction = transaction;
        _handler = handler;
        _converter = converter;
    }
    
    public async Task<Result> HandleEvent()
    {
        var messageResult = await _transaction.GetNewRecordFromTopic(_topicName);

        if (messageResult.IsFailure)
        {
            return messageResult;
        }

        var convertResult = _converter.Convert<TEvent>(messageResult.Value);

        if (convertResult.IsFailure)
        {
            return convertResult;
        }
        
        var handleResult = await _handler.HandleAsync(convertResult.Value);

        if (handleResult.IsFailure)
        {
            return handleResult.ConvertFailure<TEvent>();
        }

        var commitResult = await _transaction.Commit();

        if (commitResult.IsFailure)
        {
            return commitResult.ConvertFailure<TEvent>();
        }

        return Result.Success();
    }
}