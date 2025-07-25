using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides extension methods for handling exceptions in asynchronous task pipelines.
/// Enables fluent error recovery by providing fallback values or tasks.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Catches an exception thrown by the task and returns a fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to execute.</param>
    /// <param name="onError">A function that produces a fallback value when an exception is thrown.</param>
    /// <returns>
    /// The original task result if it succeeds, or the fallback value provided by <paramref name="onError"/> if an exception occurs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/> or <paramref name="onError"/> is null.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> Catch<T>(this Task<T> task, Func<Exception, T> onError)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (onError is null)
            throw new ArgumentNullException(nameof(onError));

        try
        {
            return await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return onError(ex);
        }
    }

    /// <summary>
    /// Catches an exception thrown by the task and returns a fallback asynchronous result.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to execute.</param>
    /// <param name="onError">A function that produces a fallback task when an exception is thrown.</param>
    /// <returns>
    /// The original task result if it succeeds, or the result of the fallback task provided by <paramref name="onError"/> if an exception occurs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/> or <paramref name="onError"/> is null.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> Catch<T>(this Task<T> task, Func<Exception, Task<T>> onError)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (onError is null)
            throw new ArgumentNullException(nameof(onError));

        try
        {
            return await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return await onError(ex).ConfigureAwait(false);
        }
    }
}
