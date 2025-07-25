using System.Diagnostics;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides fluent chaining extensions for asynchronous task pipelines.
/// Enables composing logic in a readable and expressive way.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Chains an asynchronous operation to be executed after the task resolves.
    /// </summary>
    [DebuggerStepThrough]
    public static async Task<TU> Then<T, TU>(this Task<T> task, Func<T, Task<TU>> next)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (next is null)
            throw new ArgumentNullException(nameof(next));

        var result = await task.ConfigureAwait(false);
        return await next(result).ConfigureAwait(false);
    }

    /// <summary>
    /// Chains a synchronous transformation to be applied after the task resolves.
    /// </summary>
    [DebuggerStepThrough]
    public static async Task<TU> Then<T, TU>(this Task<T> task, Func<T, TU> next)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (next is null)
            throw new ArgumentNullException(nameof(next));

        var result = await task.ConfigureAwait(false);
        return next(result);
    }

    /// <summary>
    /// Chains an asynchronous side-effect (returning Task) without altering the result type.
    /// </summary>
    [DebuggerStepThrough]
    public static async Task<T> Then<T>(this Task<T> task, Func<T, Task> next)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (next is null)
            throw new ArgumentNullException(nameof(next));

        var result = await task.ConfigureAwait(false);
        await next(result).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Chains another asynchronous operation regardless of the result from the first one.
    /// </summary>
    [DebuggerStepThrough]
    public static async Task<TU> Then<TU>(this Task task, Func<Task<TU>> next)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (next is null)
            throw new ArgumentNullException(nameof(next));

        await task.ConfigureAwait(false);
        return await next().ConfigureAwait(false);
    }

    /// <summary>
    /// Chains another task (fire-and-forget) after a Task, returning the original result.
    /// </summary>
    [DebuggerStepThrough]
    public static async Task<T> Then<T>(this Task<T> task, Func<Task> next)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));
        if (next is null)
            throw new ArgumentNullException(nameof(next));

        var result = await task.ConfigureAwait(false);
        await next().ConfigureAwait(false);
        return result;
    }
}
