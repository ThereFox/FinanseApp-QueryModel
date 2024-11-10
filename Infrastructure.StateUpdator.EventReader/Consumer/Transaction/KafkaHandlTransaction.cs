using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Infrastructure.StateUpdator.Transaction;

namespace Infrastructure.StateUpdator;

internal class KafkaHandlTransaction<TKey, TValue> : IKafkaTransaction<TValue>
{
    private readonly KafkaConsumer<TKey, TValue> _consumer;
    
    private ConsumeResult<TKey, TValue> _currentMessage;
    private bool _isWaitCommit;

    public KafkaHandlTransaction(KafkaConsumer<TKey, TValue> consumer)
    {
        _consumer = _consumer;
    }
    
    public async Task<Result<TValue>> GetNewRecordFromTopic(string topic)
    {
        if (_isWaitCommit)
        {
            throw new AggregateException("Your must commit current record");
        }
        var getNewMessageResult = await _consumer.GetNewMessageFromTopic(topic);

        if (getNewMessageResult.IsFailure)
        {
            return getNewMessageResult.ConvertFailure<TValue>();
        }
        
        var message = getNewMessageResult.Value;
        
        _currentMessage = message;
        _isWaitCommit = true;
        
        return Result.Success(message.Value);
    }

    public async Task<Result> Commit()
    {
        if (_isWaitCommit == false)
        {
            throw new AggregateException("Nothing to commit");
        }
        
        var commitResult = await _consumer.CommitEventHandl(_currentMessage);

        if (commitResult.IsFailure)
        {
            return commitResult;
        }
        
        _isWaitCommit = false;
        
        return commitResult;
    }
}