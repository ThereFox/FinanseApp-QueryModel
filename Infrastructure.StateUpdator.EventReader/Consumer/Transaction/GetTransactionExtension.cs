namespace Infrastructure.StateUpdator.Transaction;

public static class GetTransactionExtension
{
    public static IKafkaTransaction<TValue> CreateAtomarityTransaction<TKey, TValue>(
        this KafkaConsumer<TKey, TValue> consumer)
    {
        return new KafkaHandlTransaction<TKey, TValue>(consumer);
    }
}