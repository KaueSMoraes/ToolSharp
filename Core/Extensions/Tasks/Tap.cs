using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides a fluent way to execute a side-effect action on a task result without modifying the value.
/// Commonly used for logging, debugging, or analytics in functional pipelines.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Executes the specified <paramref name="action"/> using the result of the awaited task, then returns the original result.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The asynchronous task to await.</param>
    /// <param name="action">The side-effect action to execute with the task result.</param>
    /// <returns>The same result that was produced by the original task.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/> or <paramref name="action"/> is null.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> Tap<T>(this Task<T> task, Action<T> action)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (action is null)
            throw new ArgumentNullException(nameof(action));

        var result = await task.ConfigureAwait(false);

        action(result);

        return result;
    }
}
