using System.Diagnostics;
using Core.Records;

namespace Core.Extensions.Tasks;

/// <summary>
/// Provides a safe execution wrapper for asynchronous operations,
/// returning a Result object that encapsulates success, value, and errors.
/// </summary>
public static partial class TaskExtensions
{
    /// <summary>
    /// Attempts to execute an asynchronous operation and captures the result or exception.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The asynchronous task to attempt.</param>
    /// <returns>A <see cref="Result{T}"/> indicating success or failure, along with the result or exception.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="task"/> is null.
    /// </exception>
    [DebuggerStepThrough]
    public static async Task<Result<T>> Try<T>(this Task<T> task)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task));

        try
        {
            var result = await task.ConfigureAwait(false);
            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex);
        }
    }
}
