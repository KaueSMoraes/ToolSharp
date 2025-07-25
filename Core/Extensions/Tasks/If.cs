using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides a fluent conditional transformation method for asynchronous task results.
/// Executes a transformation only if a predicate is satisfied.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Conditionally transforms the result of an awaited task if the provided predicate returns true.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The asynchronous task to await.</param>
    /// <param name="predicate">A condition to evaluate against the result.</param>
    /// <param name="action">The transformation to apply if the predicate returns true.</param>
    /// <returns>
    /// The transformed result if the predicate passes; otherwise, the original result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/>, <paramref name="predicate"/>, or <paramref name="action"/> is null.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> If<T>(
        this Task<T> task,
        Func<T, bool> predicate,
        Func<T, T> action)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (predicate is null)
            throw new ArgumentNullException(nameof(predicate));
        if (action is null)
            throw new ArgumentNullException(nameof(action));

        var result = await task.ConfigureAwait(false);

        return predicate(result)
            ? action(result)
            : result;
    }
}
