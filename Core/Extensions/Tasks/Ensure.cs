using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides a fluent extension method for asserting conditions on task results.
/// Throws an exception when the condition is not met.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Asserts that a condition is true for the result of an asynchronous task.
    /// If the predicate fails, throws an <see cref="InvalidOperationException"/> with the specified message.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The asynchronous task to await.</param>
    /// <param name="predicate">The condition to evaluate against the task result.</param>
    /// <param name="message">The exception message to use if the condition fails.</param>
    /// <returns>The result of the task if the predicate passes.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/> or <paramref name="predicate"/> or <paramref name="message"/> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the predicate returns false for the result.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> Ensure<T>(this Task<T> task, Func<T, bool> predicate, string message)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (predicate is null)
            throw new ArgumentNullException(nameof(predicate));
        if (message is null)
            throw new ArgumentNullException(nameof(message));

        var result = await task.ConfigureAwait(false);

        if (!predicate(result))
            throw new InvalidOperationException(message);

        return result;
    }
}
