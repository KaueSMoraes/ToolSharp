namespace toolsharp.Extensions
{
    public record Result<T>(bool IsSuccess, T? Value, Exception? Error);

    public static class TaskExtensions
    {
        public static async Task<TU> Then<T, TU>(this Task<T> task, Func<T, Task<TU>> next)
        {
            var t = await task.ConfigureAwait(false);
            return await next(t).ConfigureAwait(false);
        }

        public static async Task Then<T>(this Task<T> task, Func<T, Task> next)
        {
            var t = await task.ConfigureAwait(false);
            await next(t).ConfigureAwait(false);
        }

        public static async Task<TU> Then<T, TU>(this Task<T> task, Func<T, TU> next)
        {
            var t = await task.ConfigureAwait(false);
            return next(t);
        }

        public static async Task<T> Tap<T>(this Task<T> task, Action<T> action)
        {
            var t = await task.ConfigureAwait(false);
            action(t);
            return t;
        }

        public static async Task<T> If<T>(this Task<T> task, Func<T, bool> predicate, Func<T, T> action)
        {
            var t = await task.ConfigureAwait(false);
            return predicate(t) ? action(t) : t;
        }

        public static async Task Match<T>(this Task<T> task, Action<T> onSuccess, Action<Exception> onError)
        {
            try
            {
                var t = await task.ConfigureAwait(false);
                onSuccess(t);
            }
            catch(Exception ex)
            {
                onError(ex);
            }
        }

        public static async Task<Result<T>> Try<T>(this Task<T> task)
        {
            try
            {
                var t = await task.ConfigureAwait(false);
                return new Result<T>(true, t, null);
            }
            catch(Exception ex)
            {
                return new Result<T>(false, default, ex);
            }
        }

        public static async Task<T> Retry<T>(Func<Task<T>> operation, int retries = 3)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(retries, 1);
            for (var i = 0; i < retries - 1; i++)
            {
                try
                {
                    return await operation().ConfigureAwait(false);
                }
                catch { }
            }
            return await operation().ConfigureAwait(false);
        }
    }
}
