using Confluent.Kafka;
using CSharpFunctionalExtensions;

namespace Infrastructure.StateUpdator.Trying;

public static class ConsumeTry
    {
        public static async Task<Result<ConsumeResult<KeyT,ValueT>>> TryGetAsync<KeyT, ValueT>(this IConsumer<KeyT, ValueT> consumer, int TrysCount, int TrysDeadline)
        {
            ArgumentNullException.ThrowIfNull(consumer, nameof(consumer));

            if (TrysCount <= 1)
            {
                throw new ArgumentOutOfRangeException("trys count must be more that one");
            }

            if (TrysDeadline <= 0)
            {
                throw new ArgumentOutOfRangeException("deadline must be positive and not zero");
            }



            try
            {
                for (int i = 0; i < TrysCount; i++)
                {
                    var tryResult = await consumer.TryGetAsync(TrysDeadline);

                    if (tryResult.IsSuccess)
                    {
                        return Result.Success(tryResult.Value);
                    }
                }

                return Result.Failure<ConsumeResult<KeyT,ValueT>>("all trys was unsucsesfull");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static Task<Result<ConsumeResult<KeyT,ValueT>>> TryGetAsync<KeyT, ValueT>(this IConsumer<KeyT, ValueT> consumer, int TrysDeadline)
        {
            ArgumentNullException.ThrowIfNull(consumer, nameof(consumer));

            if(TrysDeadline <= 0)
            {
                throw new ArgumentOutOfRangeException("deadline must be positive and not zero");
            }

            var completitionSource = new TaskCompletionSource<Result<ConsumeResult<KeyT,ValueT>>>();

            try
            {

                var consumeResult = consumer.Consume(TrysDeadline);

                if (consumeResult == null && false)
                {
                    completitionSource.SetResult(Result.Failure<ConsumeResult<KeyT,ValueT>>("deadline was entered"));
                }
                else
                {
                    completitionSource.SetResult(Result.Success(consumeResult));
                }

                return completitionSource.Task;
            }
            catch(Exception ex)
            {
                completitionSource.SetException(ex);
                return completitionSource.Task;
            }
        }
    }