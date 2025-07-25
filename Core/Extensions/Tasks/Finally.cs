using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides a fluent way to execute final logic after an asynchronous task,
/// similar to a <c>finally</c> block in exception handling.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Executes the given action after the task completes, regardless of success or failure.
    /// Unlike typical <c>finally</c> blocks, this method does not suppress exceptions from the final action.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to monitor and await.</param>
    /// <param name="action">The action to execute after task completion.</param>
    /// <returns>The result of the original task.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/> or <paramref name="action"/> is null.
    /// </exception>
    /// <exception cref="Exception">
    /// Exceptions from either the original task or the final action will be propagated. If both fail, an <see cref="AggregateException"/> will be thrown.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> Finally<T>(this Task<T> task, Action action)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (action is null)
            throw new ArgumentNullException(nameof(action));

        Exception? taskException = null;
        Exception? finalException = null;
        T? result = default;

        try
        {
            result = await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            taskException = ex;
        }

        try
        {
            action();
        }
        catch (Exception ex)
        {
            finalException = ex;
        }

        if (taskException is not null && finalException is not null)
            throw new AggregateException(taskException, finalException);

        if (finalException is not null)
            throw finalException;

        if (taskException is not null)
            throw taskException;

        return result!;
    }
}
