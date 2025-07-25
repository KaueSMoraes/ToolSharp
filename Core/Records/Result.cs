namespace Core.Records;

/// <summary>
/// Represents the outcome of an operation, capturing success state, value, and exception details.
/// </summary>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public readonly record struct Result<T>(bool IsSuccess, T? Value, Exception? Error)
{
    /// <summary>
    /// Indicates whether the result contains a value (i.e., the operation succeeded).
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result<T> Success(T value) => new(true, value, null);

    /// <summary>
    /// Creates a failed result with the given exception.
    /// </summary>
    public static Result<T> Failure(Exception error) => new(false, default, error);
}