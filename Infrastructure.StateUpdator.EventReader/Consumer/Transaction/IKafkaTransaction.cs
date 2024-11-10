using CSharpFunctionalExtensions;

namespace Infrastructure.StateUpdator.Transaction;

public interface IKafkaTransaction<TValue>
{
    public Task<Result<TValue>> GetNewRecordFromTopic(string topic);
    public Task<Result> Commit();
}