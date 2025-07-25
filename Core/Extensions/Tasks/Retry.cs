using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides retry logic for asynchronous operations that may fail intermittently.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Executes the specified asynchronous operation with retry support.
    /// If the operation fails, it will be retried up to <paramref name="retries"/> times.
    /// </summary>
    /// <typeparam name="T">The type of result returned by the operation.</typeparam>
    /// <param name="operation">The asynchronous operation to execute.</param>
    /// <param name="retries">The maximum number of attempts to perform. Must be ≥ 1.</param>
    /// <param name="delayBetweenRetries">
    /// Optional delay (in milliseconds) to wait between retries. Set to 0 to disable delay.
    /// </param>
    /// <returns>The result of a successful operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="operation"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="retries"/> is less than 1.
    /// </exception>
    /// <exception cref="Exception">
    /// Throws the last exception if all attempts fail.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> Retry<T>(
        Func<Task<T>> operation,
        int retries = 3,
        int delayBetweenRetries = 0)
    {
        if (operation is null)
            throw new ArgumentNullException(nameof(operation));

        if (retries < 1)
            throw new ArgumentOutOfRangeException(nameof(retries), "Retry count must be at least 1.");

        Exception? lastException = null;

        for (int attempt = 1; attempt <= retries; attempt++)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                lastException = ex;
                if (attempt == retries)
                    break;

                if (delayBetweenRetries > 0)
                    await Task.Delay(delayBetweenRetries).ConfigureAwait(false);
            }
        }
        throw new InvalidOperationException($"Operation failed after {retries} attempt(s).", lastException);
    }
}
