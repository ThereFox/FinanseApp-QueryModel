using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Infrastructure.StateUpdator.Trying;

namespace Infrastructure.StateUpdator;

public class KafkaConsumer<TKey, TValue>
{
    private const int TryDeadline = 2000;
    private const int TrysCount = 3;

    private readonly IConsumer<TKey, TValue> _consumer;

    public KafkaConsumer(IConsumer<TKey, TValue> consumer)
    {
        _consumer = consumer;
    }

    public async Task<Result<ConsumeResult<TKey, TValue>>> GetNewMessageFromTopic(string topic)
    {
        var readResult = await _consumer.TryGetAsync(TrysCount, TryDeadline);

        if (readResult.IsFailure)
        {
            return Result.Failure<ConsumeResult<TKey, TValue>>(readResult.Error);
        }
        
        return readResult;
    }

    public async Task<Result> CommitEventHandl(ConsumeResult<TKey, TValue> message)
    {
        try
        {
            await Task.Run(() => _consumer.Commit(message));
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
    
    
}