using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides fluent conditional exception-throwing for asynchronous task results.
/// Useful for short-circuiting pipelines based on domain rules or preconditions.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Throws an exception if the specified condition is met for the result of the awaited task.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="task">The asynchronous task to await.</param>
    /// <param name="condition">A predicate to evaluate against the result.</param>
    /// <param name="exceptionFactory">A factory that creates an exception to throw when the predicate is true.</param>
    /// <returns>The original result if the condition is not met.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/>, <paramref name="condition"/>, or <paramref name="exceptionFactory"/> is null.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when the condition evaluates to true using the provided exception factory.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<T> ThrowIf<T>(
        this Task<T> task,
        Func<T, bool> condition,
        Func<T, Exception> exceptionFactory)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (condition is null)
            throw new ArgumentNullException(nameof(condition));
        if (exceptionFactory is null)
            throw new ArgumentNullException(nameof(exceptionFactory));

        var result = await task.ConfigureAwait(false);

        if (condition(result))
            throw exceptionFactory(result);

        return result;
    }
}
